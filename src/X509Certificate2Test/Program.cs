using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace X509Certificate2Test
{
    public class Program
    {
        private static readonly string CertificateData = "-----BEGIN CERTIFICATE-----MIIDMTCCAhmgAwIBAgIRAM9ApItfG3+XzDGH5Wfr51swDQYJKoZIhvcNAQELBQAwFDESMBAGA1UEAwwJY2xvdWQubmV0MCAXDTk5MTIzMTE2MDAwMFoYDzIwOTkxMjMxMTYwMDAwWjAUMRIwEAYDVQQDDAljbG91ZC5uZXQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC8bZJ5vMc9Cawn2Sqaj/59sYgG+BXKmRAVgIaEXCTkD+CqgO3DeWncVyGLJoWfzHBNewjsgehOOhp6mCjjxj6Dp7LuK9zRZcjf6yympzkp9eEJAl5VK3crU5BdSBJ3Aqmr1Z8gl83PDZbB0BMxJlTkIKOHjApI58woY5WDLqVNSJ2tpr2aFykLQvytntDOiZYHGpwhGZDN6/RMXSyha61KsCRMyWIMvac4CW4kCZ7y/y483d93qo/r2LgLAcYFw4hwI2L0VtL6Rf8Db+huIcg/PSnqfe1nXZT8oPC6fSnwLEn3lnqu5XQLr0TCHWSuya+mLl99pwdUFCHjN59yVufJAgMBAAGjfDB6MEwGA1UdIwRFMEOAFMXJGr5wh1g74+nfV91VO74hFFbHoRikFjAUMRIwEAYDVQQDDAljbG91ZC5uZXSCEQDPQKSLXxt/l8wxh+Vn6+dbMAsGA1UdDwQEAwIBljAdBgNVHSUEFjAUBggrBgEFBQcDAQYIKwYBBQUHAwIwDQYJKoZIhvcNAQELBQADggEBACwOPfh8xHABIXZXpfaIQ/7b15tsmsJhxMtkw17J1T2NLjipzsO8PpjdXLNSsp1syf5CPqCJfiZnsFcYgIZkEaTH/hDSXqZEZroqkrIbuwm7QZ1zSNO9KqeOcgPIsO/ivXkaERveKQUJe25lXjc9UZPvynuA8IXZboppNTr6dW7nlB/jH/0hz+BwLKZ47TO8t1o5nJQYo8YrNFam565CIbt2p536Rx/hcGcN48ibU6f3CZqWLQx0wF0O/TcN5DtcJdoOrELzUs+LvCGA1RPf0LbIDcvyrRl573JoYv5I+q0c5UllqKiubM5ACYsg9nxIqmdGYWnLWjvhQZXwuyQAEgk=-----END CERTIFICATE-----";
        private static readonly string KeyData = "-----BEGIN RSA PRIVATE KEY-----MIIEpQIBAAKCAQEAvG2SebzHPQmsJ9kqmo/+fbGIBvgVypkQFYCGhFwk5A/gqoDtw3lp3FchiyaFn8xwTXsI7IHoTjoaepgo48Y+g6ey7ivc0WXI3+sspqc5KfXhCQJeVSt3K1OQXUgSdwKpq9WfIJfNzw2WwdATMSZU5CCjh4wKSOfMKGOVgy6lTUidraa9mhcpC0L8rZ7QzomWBxqcIRmQzev0TF0soWutSrAkTMliDL2nOAluJAme8v8uPN3fd6qP69i4CwHGBcOIcCNi9FbS+kX/A2/obiHIPz0p6n3tZ12U/KDwun0p8CxJ95Z6ruV0C69Ewh1krsmvpi5ffacHVBQh4zefclbnyQIDAQABAoIBAFufni+C+5CzENrJjx1cMl/1QRM+c/4xYnBKMF7RHYEmNVVDXxoDglXJqn5qy2QSOXN7hbHXU1oih7igH5XuUnybQXbtrjDGISC8ztf0jWfZb0T9uVgJzCctuY+aWZw0F8P+GqPzHPj2/x0cLBDktje9pTbmgiPETvI+xjdlKa0DckxcwQJqnocLF8tkdRj3WW9iAL9yca5Ee/6ddjjpRMbnT1bCk92Hfsy5Ew8dZ6ddh9iV3mBV2JLXi+ahktzPsEEfnCitsjJ4elmGnmTGo9MNLCs523BMUkEyEnyvoYfBfp7FdZLcvGRMcAlQEAIglicJ+SYcKwFyauebMmXbICcCgYEA3Ab8nCQ0p/TtbTYVmoix7cgbonHTXGeODy3qq8ad3h+rCko8aJkKQO+KoRbEtjDJaEfPpbmlseofwtJAClYoHqkZ2fN6ajPXOp0mIOxK8s2pWT2POBwN/EQQyXyo6sbSD3GhIgMlBg66PRkFIo3BZ9+wXglzY/6kqf6FEKBHM88CgYEA2zwH1vmGWup3tgv7ChYjJo0boQGBVBbiZL4ZxvpjBH7XoaKNye4qO6MJA9he1JS3WR0XdMHJ8aeSfyfJZZ1PmEBdXTAIkKq+oJZV3Kvx/Ul/3eUjEEsyRU2shGvYwGvxqR58PPcylZNktrQMHQvEfUvEXQ55VWwV+I/ado04WOcCgYEAsq994LRKiwgO9X1RyCExFqFHSAJWmxmCNfOdPAld1bE0L3QgYPXAbQHcPHyIHkm6l1SSXs1Ishcn2gjzdzGG/XdpBiaEiNE2/mP3Pg+Hwm2hFVc4A2JNPUxtsaPqblgu6dnm+P9CxwuY1duG3wvuQJRZ39SfAFshkOihWeJAUOMCgYEAxVqA5DCzldxD74lRb38GHYohOQsV1RDUtEO9CLYVtNJKYqLmaMURF6ZoUyHQHCXT91CM8PoSRIvJANZcIjE+mZw3b8/xpelOuVkb6g6PRKeJh8LhkaVfl6uYhYxgDrgdT1S2GludGbEZlK73yJ3zP6eZGPwSlac7EArParEt7DkCgYEAs1zhzXP4wbvFsXAr7BbGA5bHcepNDLUiTLIL/SNX2m2qkceuF81LS1wvX0fHWMKv+Ezf3ZanwcNaeu4HcSt72696twvT/APUlzCpiHNJg7hnAvP2cNxu9LDOPh3oUiE3PWg62FRrLyozLtIBvQJyvNOKpXzFUZaMFGNpmk9/NDk=-----END RSA PRIVATE KEY-----";

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(kestreOptions =>
                {
                    kestreOptions.ListenAnyIP(443, listenOptions =>
                    {
                        var cert = new X509Certificate2(Encoding.ASCII.GetBytes(CertificateData));
                        var key = RSAHelper.DecodeRSAPrivateKey(KeyData);
                        var rsa = RSA.Create(key);
                        cert = cert.CopyWithPrivateKey(rsa);
                        //uncomment this will work fine.
                        //cert = new X509Certificate2(cert.Export(X509ContentType.Pfx, "12345678"), "12345678");
                        listenOptions.UseHttps(cert);
                    });
                })
                .UseStartup<Startup>();
    }
}
