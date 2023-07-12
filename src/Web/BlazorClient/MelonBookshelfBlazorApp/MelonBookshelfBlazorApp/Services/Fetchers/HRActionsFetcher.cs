﻿namespace MelonBookshelfBlazorApp.Services.Fetchers
{
    public class HRActionsFetcher : ApiFetcher
    {
        public HRActionsFetcher(HttpClient httpClient) : base(httpClient)
        { }

        public Task<string> ConfirmRequest(string requestId, HttpContent content)
            => PutAsync(string.Format(ApiEndpoints.ApiEndpoints.HRActions.ConfirmRequest, requestId), content);

        public Task<string> RejectRequest(string requestId, HttpContent content)
            => PutAsync(string.Format(ApiEndpoints.ApiEndpoints.HRActions.RejectRequest, requestId), content);

        public Task<string> SetResourceInProgress(string resourceId, HttpContent content)
            => PutAsync(string.Format(ApiEndpoints.ApiEndpoints.HRActions.SetRequestInProgress, resourceId), content);

        public Task<string> SetResourceInDelivery(string resourceId, HttpContent content)
            => PutAsync(string.Format(ApiEndpoints.ApiEndpoints.HRActions.SetRequestInDelivery, resourceId), content);

        public Task<string> SetResourceDelivered(string resourceId, HttpContent content)
            => PutAsync(string.Format(ApiEndpoints.ApiEndpoints.HRActions.SetRequestDelivered, resourceId), content);
    }
}
