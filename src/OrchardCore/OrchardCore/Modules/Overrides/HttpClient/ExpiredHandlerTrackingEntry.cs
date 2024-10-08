// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Http;

// Thread-safety: This class is immutable.
internal sealed class ExpiredHandlerTrackingEntry
{
    private readonly WeakReference _livenessTracker;

    // IMPORTANT: don't cache a reference to `other` or `other.Handler` here.
    // We need to allow it to be collected by the GC.
    public ExpiredHandlerTrackingEntry(ActiveHandlerTrackingEntry other)
    {
        Name = other.Name;
        Scope = other.Scope;

        _livenessTracker = new WeakReference(other.Handler);
        InnerHandler = other.Handler.InnerHandler!;
    }

    public bool CanDispose => !_livenessTracker.IsAlive;

    public HttpMessageHandler InnerHandler { get; set; }

    public string Name { get; }

    public IServiceScope Scope { get; set; }
}
