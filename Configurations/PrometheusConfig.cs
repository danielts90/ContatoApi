using Prometheus;

namespace ContatoApi.Configurations
{
    public static class PrometheusExtensions
    {
        public static IApplicationBuilder UsePrometheusMetrics(this IApplicationBuilder app)
        {
            var counter = Metrics.CreateCounter("webapimetric", "Contador de requests",
                new CounterConfiguration
                {
                    LabelNames = new[] { "method", "endpoint", "status" }
                });

            app.Use((context, next) =>
            {
                counter.WithLabels(context.Request.Method, context.Request.Path, context.Response.StatusCode.ToString()).Inc();
                return next();
            });

            return app;
        }
    }
}
