<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="entitled_list.aspx.cs" Inherits="entitled_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="https://netdna.bootstrapcdn.com/font-awesome/3.2.1/css/font-awesome.min.css" rel="stylesheet" type="text/css" />

    <style>
        .row, content {
            direction: rtl;
        }

        .buttons {
            margin-top: 20px;
            margin-bottom: 20px;
        }

        .middle_div {
            position: relative;
            top: 100px;
        }

        .main_area {
            width: 50%;
        }

        .right-inner-addon i {
            position: absolute;
            right: 250px;
            top:-5px;
            padding: 10px 12px;
            pointer-events: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="admin_calendar.aspx">בית</a></li>
                <li class="active">זכאויות</li>
            </ol>
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>זכאויות תלמידים</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>

        </div>
    </section>

    <div>
        <asp:SqlDataSource ID="entitledDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="	SELECT stu_id, (Stu_FirstName + ' ' +  Stu_LastName + ' - ' + cast(stu_id as nvarchar (255)))as 'fullName' FROM Student where Stu_IsEntitled='1' order by Stu_LastName"></asp:SqlDataSource>
    </div>
    <div>
        <asp:SqlDataSource ID="UnEntitledDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="	SELECT stu_id, (Stu_FirstName + ' ' +  Stu_LastName + ' - ' + cast(stu_id as nvarchar (255)))as 'fullName' FROM Student where Stu_IsEntitled='0' order by Stu_LastName"></asp:SqlDataSource>
    </div>



    <div class="container main_area" style="margin: 0 auto; text-align: center;">

        <div class="row">
            <div class="col-sm-4 col-xs-12 col-lg-5">
                <br />

                <h3 style="color: #4e68a1">
                    <span>לא זכאים</span>
                </h3>
                <asp:ListBox ID="blackLB" runat="server" Height="300px" Width="250px" SelectionMode="Multiple" DataSourceID="UnEntitledDS" DataTextField="fullName" DataValueField="stu_id" Style="margin-top: 0px; padding: 5px; border-width: 2px; border-color: #4e68a1;"></asp:ListBox>
                <br />
                <asp:Button ID="transferAllBackBTN" runat="server" Text="נקה רשימה" OnClick="transferAllBackBTN_Click" CssClass="btn btn-primary" style="margin-bottom:50px;margin-top:20px;"/>

            </div>
            <div class="col-sm-4 col-xs-12 col-lg-2">
                <div class="middle_div">
                    <br />

                    <asp:Button ID="transferOneBTN" runat="server" Text=">" OnClick="transferOneBTN_Click" CssClass="buttons btn-social btn-facebook-filled btn-circle" />
                    <br />


                    <asp:Button ID="transferOneBackBTN" runat="server" Text="<" OnClick="transferOneBackBTN_Click" CssClass="buttons btn-social btn-facebook-filled btn-circle" />
                </div>

            </div>
            <div class="col-sm-4 col-xs-12 col-lg-5">
                <div class="right-inner-addon">
                    <i class="icon-search"></i>
                    <asp:TextBox ID="filterTB" runat="server" placeholder="חפש תלמיד" Style="width: 250px;"></asp:TextBox>
                </div>
                <h3 style="color: #4e68a1">
                    <span>זכאים</span>
                </h3>
                <asp:ListBox ID="allLB" runat="server" DataSourceID="entitledDS" DataTextField="fullName" DataValueField="stu_id" Height="300px" SelectionMode="Multiple" Width="250px" Style="margin-top: 0px; padding: 5px; border-width: 2px; border-color: #4e68a1;"></asp:ListBox>

            </div>
        </div>
    </div>

    <div>
        <asp:Label ID="lbltxt" runat="server" Text=""></asp:Label>
    </div>


    <script>
        function DoListBoxFilter(listBoxSelector, filter, keys, values) {
            var list = $(listBoxSelector);
            var selectBase = '<option value="{0}">{1}</option>';
            list.empty();
            for (i = 0; i < values.length; ++i) {
                var value = values[i];
                if (value == "" || value.toLowerCase().indexOf(filter.toLowerCase()) >= 0) {
                    var temp = '<option value="' + keys[i] + '">' + value + '</option>';
                    list.append(temp);
                }
            }
        }
        var keys = [];
        var values = [];
        var options = $('#<% = allLB.ClientID %> option');

        $.each(options, function (index, item) {
            keys.push(item.value);
            values.push(item.innerHTML);
        });
        $('#<% = filterTB.ClientID %>').keyup(function () {
            var filter = $(this).val();
            DoListBoxFilter('#<% = allLB.ClientID %>', filter, keys, values);
        });

    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="jsPlaceHolder" runat="Server">
</asp:Content>

