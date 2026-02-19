using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;

namespace CoreApiCRUD.Services
{
    public class RateLimitService
    {
        private readonly IMemoryCache _cache;
        private readonly int _requestLimit = 1;  // Max requests per time window
        private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1); // 1 minute window
        private readonly TimeSpan _blockDuration = TimeSpan.FromMinutes(1);  // Block duration after exceeding rate limit (e.g., 5 minutes)
        private readonly string BlockedIpsKey = "BlockedIps";  // Cache key for blocked IPs

        public RateLimitService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool IsRequestAllowed(string key)
        {
            var now = DateTime.UtcNow;

            // Check if the IP is blocked
            if (IsIpBlocked(key))
            {
                return false; // If IP is blocked, deny the request
            }

            var requestHistoryKey = $"{key}-history";
            if (!_cache.TryGetValue(requestHistoryKey, out ConcurrentQueue<DateTime> requestHistory))
            {
                requestHistory = new ConcurrentQueue<DateTime>();
            }

            // Remove expired requests
            while (requestHistory.TryPeek(out DateTime timestamp) && now - timestamp > _timeWindow)
            {
                requestHistory.TryDequeue(out _);
            }

            // Check if the request limit has been exceeded
            if (requestHistory.Count >= _requestLimit)
            {
                // Block the IP temporarily if the limit is exceeded
                BlockIp(key);
                return false; // Limit exceeded
            }

            // Record the current request timestamp
            requestHistory.Enqueue(now);
            _cache.Set(requestHistoryKey, requestHistory, _timeWindow);

            return true;
        }

        private void BlockIp(string ip)
        {
            // Retrieve the list of blocked IPs
            var blockedIps = _cache.GetOrCreate(BlockedIpsKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _blockDuration;
                return new HashSet<string>();
            });

            // Add the IP to the blocked list
            blockedIps.Add(ip);

            // Set the cache with expiration time for the blocked IPs
            _cache.Set(BlockedIpsKey, blockedIps, _blockDuration);
        }

        private bool IsIpBlocked(string ip)
        {
            // Retrieve the list of blocked IPs
            if (_cache.TryGetValue(BlockedIpsKey, out HashSet<string> blockedIps))
            {
                return blockedIps.Contains(ip);
            }

            return false;
        }
    }

}
