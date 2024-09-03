using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberTrucking.Services.Helpers
{
    public static class EmailBuilder
    {
        public static string BuildForgotPasswordEmailBody(string email)
        {
            return $@"
                <html>
                    <head>
                    </head>
                    <body style=""margin: 0; padding: 0; font - family: Arial, Helvetica, sans - serif; "">
                        <div style=""height: auto; background: Linear - gradient(to top, #c9c9ff 50%, #6e6ef6 90%) no-repeat; width:400px; padding:30px"">
                            <div>
                                 <h1>Your Password</h1>
                                 <hr>
                                 <p>You're receiving this e-mail because you requested a password reset for your account.</p> 
                                 
                                 <p>Link: <a href=""http://localhost:4200/reset-password?email={email}"">Reset Password!</a></p>
                                 <p>Kind Regards, <br><br>
                                 UBT Team</p>
                            </div
                        </div>
                    </body>
                </html>
            ";
        }
    }
}
