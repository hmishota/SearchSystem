using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SearchTool
{
    class MultitaskingSearcher
    {
        public void t(int id)
        {
            var c = new CancellationTokenSource();

            var task = GetT(null, c.Token);

            c.Cancel();


            



            try
            {

            }
            catch (Exception e)
            {

                

                throw new Exception($"{id}", e);
            }
        }


        public async Task<string> GetT(IEnumerable<int> collection, CancellationToken cancel)
        {

            foreach (var item in collection)
            {
                cancel.ThrowIfCancellationRequested();


            }

            return  "";
        }
    }
}
