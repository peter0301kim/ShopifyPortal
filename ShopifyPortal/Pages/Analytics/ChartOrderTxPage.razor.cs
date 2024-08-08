using ShopifyPortal.Helpers;
using ShopifyPortal.Integration.PortalDb;
using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.Integration.PortalDb.Services;
using MudBlazor;

namespace ShopifyPortal.Pages.Analytics;

public partial class ChartOrderTxPage
{
    private int ChartIndex = -1; //default value cannot be 0 -> first selectedindex is 0.
    public ChartOptions Options = new ChartOptions()
    {
         InterpolationOption = InterpolationOption.NaturalSpline,
    };

    public List<ChartSeries> Chart_Org_Year_Month_TxCount = new List<ChartSeries>();
    public List<ChartSeries> Chart_Org_Year_Month_BrainzPoint = new List<ChartSeries>();

    public string[] XAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    bool IsProgress { get; set; } = false;
    public ChartOrderTxPage()
    {
    }

    protected override async Task OnInitializedAsync()
    {
    }

    protected override async Task OnParametersSetAsync()
    {
        MakeChart_Org_Year_Month_TxCount();
        MakeChart_Org_Year_Month_BrainzPoint();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
        }
    }




    private void MakeChart_Org_Year_Month_TxCount()
    {
        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        var chart_Org_Year_Month_TxCount = portalDbService.GetOrderTxs_GroupByOrg_Year_Month_TxCount("2023");
        var distinctOrgs = chart_Org_Year_Month_TxCount.Select(o => o.GroupNyName).Distinct();

        Chart_Org_Year_Month_TxCount = new List<ChartSeries>();

        foreach (var org in distinctOrgs)
        {
            ChartSeries chartSeries = new ChartSeries();
            chartSeries.Name = org;

            double[] txOrderCountValues = new double[12];
            var filterdDatas = (from chart in chart_Org_Year_Month_TxCount where chart.GroupNyName == org select chart).ToList().OrderBy(e => e.Month);

            foreach (var data in filterdDatas)
            {
                switch (data.Month)
                {
                    case 1: { txOrderCountValues[0] = data.Value; break; }
                    case 2: { txOrderCountValues[1] = data.Value; break; }
                    case 3: { txOrderCountValues[2] = data.Value; break; }
                    case 4: { txOrderCountValues[3] = data.Value; break; }
                    case 5: { txOrderCountValues[4] = data.Value; break; }
                    case 6: { txOrderCountValues[5] = data.Value; break; }
                    case 7: { txOrderCountValues[6] = data.Value; break; }
                    case 8: { txOrderCountValues[7] = data.Value; break; }
                    case 9: { txOrderCountValues[8] = data.Value; break; }
                    case 10: { txOrderCountValues[9] = data.Value; break; }
                    case 11: { txOrderCountValues[10] = data.Value; break; }
                    case 12: { txOrderCountValues[11] = data.Value; break; }

                    default:
                        {
                            break;
                        }
                }


            }

            chartSeries.Data = txOrderCountValues;

            Chart_Org_Year_Month_TxCount.Add(chartSeries);
        }
    }

    private void MakeChart_Org_Year_Month_BrainzPoint()
    {
        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        var chart_Org_Year_Month_BrainzPoint = portalDbService.GetOrderTxs_GroupByOrg_Year_Month_BrainzPoint("2023");
        var distinctOrgs = chart_Org_Year_Month_BrainzPoint.Select(o => o.GroupNyName).Distinct();

        Chart_Org_Year_Month_BrainzPoint = new List<ChartSeries>();

        foreach (var org in distinctOrgs)
        {
            ChartSeries chartSeries = new ChartSeries();
            chartSeries.Name = org;

            double[] txOrderCountValues = new double[12];
            var filterdDatas = (from chart in chart_Org_Year_Month_BrainzPoint where chart.GroupNyName == org select chart).ToList().OrderBy(e => e.Month);

            foreach (var data in filterdDatas)
            {
                switch (data.Month)
                {
                    case 1: { txOrderCountValues[0] = data.Value; break; }
                    case 2: { txOrderCountValues[1] = data.Value; break; }
                    case 3: { txOrderCountValues[2] = data.Value; break; }
                    case 4: { txOrderCountValues[3] = data.Value; break; }
                    case 5: { txOrderCountValues[4] = data.Value; break; }
                    case 6: { txOrderCountValues[5] = data.Value; break; }
                    case 7: { txOrderCountValues[6] = data.Value; break; }
                    case 8: { txOrderCountValues[7] = data.Value; break; }
                    case 9: { txOrderCountValues[8] = data.Value; break; }
                    case 10: { txOrderCountValues[9] = data.Value; break; }
                    case 11: { txOrderCountValues[10] = data.Value; break; }
                    case 12: { txOrderCountValues[11] = data.Value; break; }

                    default:
                        {
                            break;
                        }
                }


            }

            chartSeries.Data = txOrderCountValues;

            Chart_Org_Year_Month_BrainzPoint.Add(chartSeries);
        }
    }
}