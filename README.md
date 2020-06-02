# swagdoc
基于swagger自动生成word文档

//注入AddSwaggerGen服务
 public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "生成api文档",
                        Version = "v1",
                        Description = "api文档",
                        Contact = new OpenApiContact { Name = "Contact", Url = new Uri("http://test.com") },
                        TermsOfService = new Uri("http://tempuri.org/terms")
                    }
                );
            });
        }
       
       //添加UseSwaggerUI中间件
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSwagger("swagger/{documentName}/swagger.json");
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = ""; // serve the UI at root
                c.SwaggerEndpoint("swagger/v1/swagger.json", "V1 Docs");
            });

        }
        
       //Controller 生成word文档
          public ISwaggerProvider SwaggerGenerator { get; }
        public IOptions<SpireDocHelper> SpireDocHelper { get; }
        public WeatherForecastController(ISwaggerProvider SwaggerGenerator, IOptions<SpireDocHelper> SpireDocHelper)
        {
            this.SwaggerGenerator = SwaggerGenerator;
            this.SpireDocHelper = SpireDocHelper;
           
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns>返回车</returns>
        [HttpGet("/GetSwDoc")]
        public IActionResult GetSwDoc()
        {
            var document = SwaggerGenerator.GetSwagger("v1");
            string memi = string.Empty;
            var stream = SpireDocHelper.Value.GetSwDoc(document, out memi);
            return File(stream, memi, "api简化文档");
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns>返回车</returns>
        [HttpGet("/GetSwDocByPath")]
        public IActionResult GetSwDocByPath()
        {
            var document = SwaggerGenerator.GetSwagger("v1");
            string memi = string.Empty;
             var stream = new SpireDocHelper().GetSwDoc(document, out memi, $"Templates\\SwaggerDoc.cshtml");
             return File(stream, memi, "api简化文档");
        }
        
