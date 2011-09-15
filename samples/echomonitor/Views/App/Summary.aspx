<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<echomonitor.Models.AppViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.Name %> Summary
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1><%= Model.Name %> Summary</h1>

    <h2>Sessions (Today)</h2>
    <div id="sessionsTodayGraph" style="width: 400px; height: 300px;"></div>
    <h2>Active Users <span id="activeUserCount"></span></h2>
    <div id="activeUsers" style="width: 400px; height: 300px;">
        <ul></ul>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptPlaceHolder" runat="server">
    <script src="../../Scripts/flot-0.5/jquery.flot.js" type="text/javascript"></script>

     <!--[if IE]><script src="../../Scripts/flot-0.5/excanvas.js" type="text/javascript"></script><![endif]-->

    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            SessionsTodayGraph.Start();
            ActiveUsersList.Start();
        });

        var SessionsTodayGraph = new function() {
            var self = SessionsTodayGraph;
            // poll interval in seconds
            this.PollInterval = <%= Model.PollInterval %>;

            this.Render = function() {
                $.getJSON("/session/daysummary?app=<%= Model.Name %>", function(sessionData) {
                    var options = {
                        bars: { show: true },
                        lines: { show: false },
                        points: { show: false },
                        xaxis: { ticks: [[0, "00:00"], [16, "04:00"], [32, "08:00"], [48, "12:00"], [64, "16:00"], [80, "20:00"], [96, "24:00"]] },
                        yaxis: { minTickSize: 1 }
                    };
                    $.plot($("#sessionsTodayGraph"), [sessionData], options);
                });
            };

            this.Start = function() {
                var self = SessionsTodayGraph;
                self.Poll();
            };

            this.Poll = function() {
                var self = SessionsTodayGraph;
                self.Render();
                self.pollHand = setTimeout(self.Poll, self.PollInterval * 1000);
            };

        }

        var ActiveUsersList = new function() {
            var self = ActiveUsersList;
            this.PollInterval = <%= Model.PollInterval %>;

            this.Render = function() {
                $.getJSON("/session/activeusers?app=<%=Model.Name %>", function(usersList) {
                    $('#activeUsers ul').empty();
                    for (index in usersList) {
                        $('#activeUsers ul').append('<li>' + usersList[index] + '</li>');
                    };
                    $('#activeUserCount').text('(' + usersList.length + ')');
                });
            }

            this.Start = function() {
                var self = ActiveUsersList;
                self.Poll();
            };

            this.Poll = function() {
                var self = ActiveUsersList;
                self.Render();
                self.pollHand = setTimeout(self.Poll, self.PollInterval * 1000);
            };
        }
    </script>

</asp:Content>
