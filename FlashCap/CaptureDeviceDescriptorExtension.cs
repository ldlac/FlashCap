﻿////////////////////////////////////////////////////////////////////////////
//
// FlashCap - Independent camera capture library.
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under Apache-v2: https://opensource.org/licenses/Apache-2.0
//
////////////////////////////////////////////////////////////////////////////

using FlashCap.FrameProcessors;
using System.Threading.Tasks;

namespace FlashCap
{
    public enum HandlerStrategies
    {
        IgnoreDropping,
        Queuing,
        Scattering,
    }

    public delegate void PixelBufferArrivedDelegate(
        PixelBuffer buffer);

#if NET35_OR_GREATER || NETSTANDARD || NETCOREAPP
    public delegate Task PixelBufferArrivedTaskDelegate(
        PixelBuffer buffer);

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
    public delegate ValueTask PixelBufferArrivedValueTaskDelegate(
        PixelBuffer buffer);
#endif

    public static class CaptureDeviceDescriptorExtension
    {
        public static Task<CaptureDevice> OpenAsync(
            this CaptureDeviceDescriptor descriptor,
            VideoCharacteristics characteristics,
            PixelBufferArrivedDelegate pixelBufferArrived) =>
            descriptor.OpenWithFrameProcessorAsync(
                characteristics, true,
                new DelegatedIgnoreDroppingProcessor(pixelBufferArrived));

        public static Task<CaptureDevice> OpenAsync(
            this CaptureDeviceDescriptor descriptor,
            VideoCharacteristics characteristics,
            bool transcodeIfYUV,
            HandlerStrategies handlerStrategy,
            PixelBufferArrivedDelegate pixelBufferArrived) =>
            descriptor.OpenWithFrameProcessorAsync(
                characteristics, transcodeIfYUV,
                handlerStrategy switch
                {
                    HandlerStrategies.Queuing =>
                        new DelegatedQueuingProcessor(pixelBufferArrived),
                    HandlerStrategies.Scattering =>
                        new DelegatedScatteringProcessor(pixelBufferArrived),
                    _ =>
                        new DelegatedIgnoreDroppingProcessor(pixelBufferArrived),
                });

        public static Task<CaptureDevice> OpenAsync(
            this CaptureDeviceDescriptor descriptor,
            VideoCharacteristics characteristics,
            PixelBufferArrivedTaskDelegate pixelBufferArrived) =>
            descriptor.OpenWithFrameProcessorAsync(
                characteristics, true,
                new DelegatedIgnoreDroppingTaskProcessor(pixelBufferArrived));

        public static Task<CaptureDevice> OpenAsync(
            this CaptureDeviceDescriptor descriptor,
            VideoCharacteristics characteristics,
            bool transcodeIfYUV,
            HandlerStrategies handlerStrategy,
            PixelBufferArrivedTaskDelegate pixelBufferArrived) =>
            descriptor.OpenWithFrameProcessorAsync(
                characteristics, transcodeIfYUV,
                handlerStrategy switch
                {
                    HandlerStrategies.Queuing =>
                        new DelegatedQueuingTaskProcessor(pixelBufferArrived),
                    HandlerStrategies.Scattering =>
                        new DelegatedScatteringTaskProcessor(pixelBufferArrived),
                    _ =>
                        new DelegatedIgnoreDroppingTaskProcessor(pixelBufferArrived),
                });

#if NET45_OR_GREATER || NETSTANDARD || NETCOREAPP
        public static Task<CaptureDevice> OpenAsync(
            this CaptureDeviceDescriptor descriptor,
            VideoCharacteristics characteristics,
            PixelBufferArrivedValueTaskDelegate pixelBufferArrived) =>
            descriptor.OpenWithFrameProcessorAsync(
                characteristics, true,
                new DelegatedIgnoreDroppingValueTaskProcessor(pixelBufferArrived));

        public static Task<CaptureDevice> OpenAsync(
            this CaptureDeviceDescriptor descriptor,
            VideoCharacteristics characteristics,
            bool transcodeIfYUV,
            HandlerStrategies handlerStrategy,
            PixelBufferArrivedValueTaskDelegate pixelBufferArrived) =>
            descriptor.OpenWithFrameProcessorAsync(
                characteristics, transcodeIfYUV,
                handlerStrategy switch
                {
                    HandlerStrategies.Queuing =>
                        new DelegatedQueuingValueTaskProcessor(pixelBufferArrived),
                    HandlerStrategies.Scattering =>
                        new DelegatedScatteringValueTaskProcessor(pixelBufferArrived),
                    _ =>
                        new DelegatedIgnoreDroppingValueTaskProcessor(pixelBufferArrived),
                });
#endif
    }
#endif
}
