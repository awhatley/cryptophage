using System;
using System.Threading;

namespace Cryptophage
{
    /// <summary>
    /// Provides a generic <see cref="IAsyncResult"/> implementation for use by asynchronous
    /// methods that return a value.
    /// </summary>
    /// <typeparam name="T">The type of the return value for the asynchronous method.</typeparam>
    public class AsyncResult<T> : AsyncResult
    {
        private T _returnValue;

        /// <summary>
        /// Gets the return value of the asynchronous operation, waiting for the operation to
        /// complete. If the operation threw an exception, accessing this property will throw 
        /// that exception. 
        /// </summary>
        public T ReturnValue { get { Wait(); return _returnValue; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncResult{T}"/> class with the specified
        /// callback and state values.
        /// </summary>
        /// <param name="callback">An optional asynchronous callback, to be called when 
        /// the asynchronous operation is complete.</param>
        /// <param name="state">A user-provided object that distinguishes this particular 
        /// asynchronous operation from other operations.</param>
        public AsyncResult(AsyncCallback callback, object state) : base(callback, state) { }

        /// <summary>
        /// Indicates that the asynchronous operation has completed with the specified return value.
        /// </summary>
        /// <param name="returnValue">The return value of the asynchronous operation.</param>
        public void Complete(T returnValue)
        {
            _returnValue = returnValue;
            Complete();
        }
    }

    /// <summary>
    /// Provides a generic <see cref="IAsyncResult"/> implementation for use by asynchronous
    /// methods that do not return a value.
    /// </summary>
    public class AsyncResult : IAsyncResult
    {
        private readonly AsyncCallback _callback;
        private readonly object _asyncState;
        private readonly Lazy<ManualResetEvent> _waitHandle;
        private int _completed;
        private Exception _exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncResult{T}"/> class with the specified
        /// callback and state values.
        /// </summary>
        /// <param name="callback">An optional asynchronous callback, to be called when 
        /// the asynchronous operation is complete.</param>
        /// <param name="state">A user-provided object that distinguishes this particular 
        /// asynchronous operation from other operations.</param>
        public AsyncResult(AsyncCallback callback, object state)
        {
            _callback = callback;
            _asyncState = state;
            _waitHandle = new Lazy<ManualResetEvent>(() => new ManualResetEvent(IsCompleted));
        }

        /// <summary>
        /// Indicates that the asynchronous operation has completed.
        /// </summary>
        public void Complete()
        {
            if(Interlocked.Exchange(ref _completed, 1) != 0)
                throw new InvalidOperationException("An asynchronous operation can only complete once.");

            if(_waitHandle.IsValueCreated) 
                _waitHandle.Value.Set();

            if(_callback != null) 
                _callback(this);
        }

        /// <summary>
        /// Indicates that the asynchronous operation threw an exception.
        /// </summary>
        public void Throw(Exception exception)
        {
            _exception = exception;
            Complete();
        }

        /// <summary>
        /// Waits for the asynchronous operation to complete. If the operation threw an exception,
        /// calling this method will throw that exception.
        /// </summary>
        public void Wait()
        {
            if(!IsCompleted)
            {
                try
                {
                    AsyncWaitHandle.WaitOne();
                }
                
                finally
                {
                    AsyncWaitHandle.Close();
                }
            }

            if(_exception != null) 
                throw _exception;
        }

        private WaitHandle AsyncWaitHandle
        {
            get { return _waitHandle.Value; }
        }

        private bool IsCompleted
        {
            get { return _completed != 0; }
        }

        #region Implementation of IAsyncResult

        object IAsyncResult.AsyncState
        {
            get { return _asyncState; }
        }

        bool IAsyncResult.CompletedSynchronously
        {
            get { return false; }
        }

        WaitHandle IAsyncResult.AsyncWaitHandle
        {
            get { return AsyncWaitHandle; }
        }

        bool IAsyncResult.IsCompleted
        {
            get { return IsCompleted; }
        }

        #endregion
    }
}