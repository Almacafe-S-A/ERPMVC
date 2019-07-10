using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntegracionBalanza.Utils
{
    //public class PeriodicTask
    //{
    //    public static async Task Run(Action action, TimeSpan period, CancellationToken cancellationToken)
    //    {
    //        while (!cancellationToken.IsCancellationRequested)
    //        {
    //            await Task.Delay(period, cancellationToken);


    //            if (!cancellationToken.IsCancellationRequested)
    //                action();
    //        }
    //    }

    //    public static Task Run(Action action, TimeSpan period)
    //    {
    //        return  Run(action, period, CancellationToken.None);
    //    }
    //}


    //Adaptacion
    public static class PeriodicTask
    {
        public static async Task Run(
            Func<Task> action,
            TimeSpan period,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            while (!cancellationToken.IsCancellationRequested)
            {

                Stopwatch stopwatch = Stopwatch.StartNew();

                if (!cancellationToken.IsCancellationRequested)
                    await action();

                stopwatch.Stop();

                await Task.Delay(period - stopwatch.Elapsed, cancellationToken);
            }
        }
    }


}
