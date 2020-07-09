using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DNF.Projects
{
    public class Interop
    {
        private readonly IJSRuntime _jsRuntime;

        public Interop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task CreateChart(string divid, string type, string[] labels, object[] datasets, object options)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "DNF.Projects.createChart",
                    divid, type, (object) labels, (object) datasets, options);
            }
            catch
            {
                // handle exception
            }
        }
    }
}

