using System;
using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Advanced
{
    /// <summary>
    ///     IDisposableのサンプルです。
    /// </summary>
    /// <remarks>
    ///     以下の記事を見て作成したサンプル。
    ///     http://www.codeproject.com/Tips/458846/Using-using-Statements-DisposalAccumulator
    /// </remarks>
    [Sample]
    public class DisposableSamples01 : IExecutable
    {
        public void Execute()
        {
            //
            // 通常パターン.
            //
            using (var disposable1 = new Disposable1())
            {
                using (var disposable2 = new Disposable2())
                {
                    using (var disposable3 = new Disposable3())
                    {
                        Output.WriteLine("Dispose Start..");
                    }
                }
            }

            //
            // 通常パターン: DisposableManager利用.
            //
            using (var manager = new DisposableManager())
            {
                manager.Add(new Disposable1());
                manager.Add(new Disposable2());
                manager.Add(new Disposable3());

                Output.WriteLine("Dispose Start..");
            }

            //
            // 条件が存在し、作成されないオブジェクトが存在する可能性がある場合.
            //
            Disposable1 dispose1 = null;
            Disposable2 dispose2 = null;
            Disposable3 dispose3 = null;

            var isDispose2Create = false;
            try
            {
                dispose1 = new Disposable1();

                if (isDispose2Create)
                {
                    dispose2 = new Disposable2();
                }

                dispose3 = new Disposable3();
            }
            finally
            {
                Output.WriteLine("Dispose Start..");
                DisposeIfNotNull(dispose1);
                DisposeIfNotNull(dispose2);
                DisposeIfNotNull(dispose3);
            }


            //
            // 条件あり: DisposableManager利用.
            //
            dispose1 = null;
            dispose2 = null;
            dispose3 = null;

            using (var manager = new DisposableManager())
            {
                dispose1 = manager.Add(new Disposable1());

                if (isDispose2Create)
                {
                    dispose2 = manager.Add(new Disposable2());
                }

                dispose3 = manager.Add(new Disposable3());

                Output.WriteLine("Dispose Start..");
            }
        }

        private void DisposeIfNotNull(IDisposable disposableObject)
        {
            if (disposableObject == null)
            {
                return;
            }

            disposableObject.Dispose();
        }

        private class DisposableManager : IDisposable
        {
            private readonly Stack<IDisposable> _disposables;
            private bool _isDisposed;

            public DisposableManager()
            {
                _disposables = new Stack<IDisposable>();
                _isDisposed = false;
            }

            public void Dispose()
            {
                _disposables.ToList().ForEach(disposable => disposable.Dispose());
                _disposables.Clear();

                _isDisposed = true;
            }

            public T Add<T>(T disposableObject) where T : IDisposable
            {
                Defence();

                if (disposableObject != null)
                {
                    _disposables.Push(disposableObject);
                }

                return disposableObject;
            }

            private void Defence()
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException("Cannot access a disposed object.");
                }
            }
        }

        private class Base : IDisposable
        {
            public void Dispose()
            {
                Output.WriteLine("[{0}] Disposed...", GetType().Name);
            }
        }

        private class Disposable1 : Base
        {
        }

        private class Disposable2 : Base
        {
        }

        private class Disposable3 : Base
        {
        }
    }
}