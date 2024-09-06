<%@ Page Title="" Language="C#" MasterPageFile="~/ContactBookInternalMasterPage.master" AutoEventWireup="true" CodeFile="Default_ContactBookInternalver2.aspx.cs" Inherits="Default_ContactBookInternal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="Server">

    <link href="/css/internal/pageIndexInternal.min.css" rel="stylesheet" />
    <script src="../js/hajs/jquery.js"></script>
    <script src="../js/hajs/jquery.vticker-min.js"></script>
    <style>
        .content-headertl {
            padding-bottom: 0.3rem;
            border-bottom: 3px solid #8f0100;
        }

            .content-headertl span {
                box-shadow: rgb(0 0 0 / 4%) 4px -7px 6px, rgb(0 0 0 / 14%) -4px -5px 14px;
                border-top-right-radius: 1rem;
                border-bottom-left-radius: 0;
                border-bottom-right-radius: 0;
                background: #8f0100;
                color: white;
                font-size: 0.7rem;
                padding: 0.5rem;
                font-weight: 600;
                text-transform: uppercase;
            }

        .table-thead-red thead tr th:first-child {
            border-top-left-radius: unset;
        }

        .table-thead-red thead tr th:last-child {
            border-top-right-radius: unset;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Menu" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopWrapper" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Wrapper" runat="Server">
    <input type="text" id="subMenu" value="1" hidden />
    <div class="loading" id="img-loading-icon" style="display: none">
        <div class="loading">Loading&#8230;</div>
    </div>
    <div class="container-fluid">
        <div class="section-banner">
            <img src="/images/tam_nhin_su_menh.jpg" />
        </div>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-8">
                <div class="section-tasks-months block-card">
                    <div class="block-header">
                        <span>DANH SÁCH CÔNG VIỆC</span>
                        <div class="">
                            <a href="/admin-xem-lich-cong-tac-hang-tuan-trung-hoc-version2" class="btn btn-sm btn-outline-primary" id="btnTuan" runat="server">Lịch công tác tuần</a>
                            <a href="/admin-xem-lich-cong-tac-thang-version2" id="btnThang" class="btn btn-sm btn-outline-primary" runat="server">Lịch công tác tháng</a>
                        </div>
                    </div>
                    <div class="tasks-months-content">
                        <%-- Danh sách công việc tháng --%>
                        <div class="table-responsive task-table table-thead-red">
                            <table class="table table-bordered ">
                                <thead>
                                    <tr>
                                        <th>Công việc</th>
                                        <th>Ngày</th>
                                        <th style="width: 80px;">Trạng thái</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="rpDanhSachCongViecThang">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Eval("noidung") %></td>
                                                <td><%# Convert.ToDateTime(Eval("lichcongtaccanhan_ngaytao")).ToString("dd/MM/yyyy") %></td>
                                                <td>
                                                    <span class="task-status <%#Eval("styleColor") %>"><%#Eval("lichcongtaccanhan_tinhtrang") %></span>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <%-- <tr>
             <td>Lập kế hoạch ngoại khóa ngày hội STEAM</td>
             <td>15/10/2024</td>
             <td class="task-status in-progress">Đang làm</td>
         </tr>
         <tr>
             <td>Nhập nhận xét thường xuyên lớp 6/2, 10/3, 10/4</td>
             <td>09/10/2024</td>
             <td class="task-status overdue">Quá hạn</td>
         </tr>
         <tr>
             <td>Soạn đề luyện tập tự học tuần 12</td>
             <td>20/10/2024</td>
             <td class="task-status in-progress">Đang làm</td>
         </tr>
         <tr>
             <td>Nhập lịch báo giảng tuần 12</td>
             <td>10/10/2024</td>
             <td class="task-status completed">Hoàn thành</td>
         </tr>--%>
                                </tbody>
                            </table>
                        </div>
                        <!-- Modal -->
                        <asp:Repeater runat="server" ID="rpModalDanhSachCongViecThang">
                            <ItemTemplate>
                                <div class="modal fade" id="exampleModal_<%#Eval("lichcongtaccanhan_id") %>" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-lg" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title" id="exampleModalLabel">Công việc chi tiết</h5>
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true">&times;</span>
                                                </button>
                                            </div>
                                            <div class="modal-body">
                                                <object id="pdf_viewbanhanh" data="<%#Eval("lichcongtaccanhan_file") %>" type="application/pdf" width="100%" height="450px">
                                                    <p>Alternative text - include a link <a href="<%#Eval("lichcongtaccanhan_file") %>">to the PDF!</a></p>
                                                </object>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div style="display: none">
                                    <input type="text" id="txtCongViec_id" value="" runat="server" />
                                    <asp:Button Text="text" runat="server" ID="btnLuuLichSu" OnClick="btnLuuLichSu_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <%-- Danh sách công việc tuần --%>
                        <asp:Repeater runat="server" ID="rpThoiGianTrongTuan" OnItemDataBound="rpThoiGianTrongTuan_ItemDataBound">
                            <ItemTemplate>
                                <div class="task-days" style="<%#Eval("style") %>"><%#Eval("thu") %>, <%#Eval("ngay") %></div>
                                <div class="table-responsive task-table">
                                    <table class="table table-bordered mb-0">
                                        <tbody>
                                            <asp:Repeater runat="server" ID="rpDanhSachCongViecTuan">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%#Eval("lichsucongviec_tennoidungcongviec") %></td>
                                                        <td style="width: 80px;"><span class="task-status <%#Eval("styleColor") %>"><%#Eval("lichsucongviec_tinhtrang") %></span> </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <%--<tr>
             <td>Nộp hồ sơ kiểm tra giữa kì I khối 10</td>
             <td class="task-status not-done">Chưa thực hiện</td>
         </tr>
         <tr>
             <td>Đăng kí dự giờ</td>
             <td class="task-status not-viewed">Chưa xem</td>
         </tr>--%>
                                        </tbody>
                                    </table>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div class="footer-note">Danh sách này báo đầu mục công việc cá nhân trong vòng 1 tháng và báo trước các việc ưu tiên của 2 tháng tiếp theo; công việc ưu tiên là công việc có dấu ★</div>
                    </div>
                </div>
                <div class="section-schedule block-card">
                    <div class="block-header">
                        <span>THỜI KHÓA BIỂU</span>
                        <a href="/admin-thoi-khoa-bieu-trung-hoc-version2" class="btn btn-sm btn-outline-primary" id="btnThoiKhoaBieu" runat="server">Thời khóa biểu toàn trường</a>
                    </div>
                    <div id="div_ThoiKhoaBieuGiaoVien" runat="server">
                        <%--<div class="heading-section">
    <h3 class="mb-4">THỜI KHÓA BIỂU</h3>
</div>--%>
                        <div class="block-time-table">
                            <div class="table-responsive">
                                <table class="table mb-0">
                                    <thead>
                                        <tr>
                                            <th>Tiết</th>
                                            <th>Thứ 2</th>
                                            <th>Thứ 3</th>
                                            <th>Thứ 4</th>
                                            <th>Thứ 5</th>
                                            <th>Thứ 6</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>0</td>
                                            <asp:Repeater runat="server" ID="rpTiet0">
                                                <ItemTemplate>
                                                    <td><%#Eval("mon_name") %> - <%#Eval("lop_name") %></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                        <tr>
                                            <td>1</td>
                                            <asp:Repeater runat="server" ID="rpTiet1">
                                                <ItemTemplate>
                                                    <td><%#Eval("mon_name") %> - <%#Eval("lop_name") %></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                        <tr>
                                            <td>2</td>
                                            <asp:Repeater runat="server" ID="rpTiet2">
                                                <ItemTemplate>
                                                    <td><%#Eval("mon_name") %> - <%#Eval("lop_name") %></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                        <tr>
                                            <td>3</td>
                                            <asp:Repeater runat="server" ID="rpTiet3">
                                                <ItemTemplate>
                                                    <td><%#Eval("mon_name") %> - <%#Eval("lop_name") %></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                        <tr>
                                            <td>4</td>
                                            <asp:Repeater runat="server" ID="rpTiet4">
                                                <ItemTemplate>
                                                    <td><%#Eval("mon_name") %> - <%#Eval("lop_name") %></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                        <tr>
                                            <td>5</td>
                                            <asp:Repeater runat="server" ID="rpTiet5">
                                                <ItemTemplate>
                                                    <td><%#Eval("mon_name") %> - <%#Eval("lop_name") %></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                        <tr>
                                            <td>6</td>
                                            <asp:Repeater runat="server" ID="rpTiet6">
                                                <ItemTemplate>
                                                    <td><%#Eval("mon_name") %> - <%#Eval("lop_name") %></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                        <tr>
                                            <td>7</td>
                                            <asp:Repeater runat="server" ID="rpTiet7">
                                                <ItemTemplate>
                                                    <td><%#Eval("mon_name") %> - <%#Eval("lop_name") %></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                        <tr>
                                            <td>8</td>
                                            <asp:Repeater runat="server" ID="rpTiet8">
                                                <ItemTemplate>
                                                    <td><%#Eval("mon_name") %> - <%#Eval("lop_name") %></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="section-notification">
                    <div class="notification-header">
                        <div class="notification-header__title">THÔNG BÁO NỘI BỘ</div>
                    </div>
                    <div class="notification-content" id="menu-alert">
                        <ul>
                            <asp:Repeater runat="server" ID="rpThongBao">
                                <ItemTemplate>
                                    <li>
                                        <div class="--status-new" style="<%#Eval("style") %>">
                                            <img src="/images/new-1.gif">
                                        </div>
                                        <a class="notification-item" href="/admin-file-huong-dan-danh-gia-<%#Eval("thongbao_id") %>">
                                            <div class="notification-item__title">
                                                <i class="fa fa-angle-right" aria-hidden="true"></i>&nbsp;<%#Eval("thongbao_name") %>
                                            </div>
                                            <div class="notification-item__other">
                                                <span class="--level"><%#Eval("thongbao_cap")%></span>
                                                <span class="--date"><%#Convert.ToDateTime(Eval("thongbao_datecreate")).ToShortDateString()%></span>
                                            </div>
                                        </a>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="col-12">
                <div class="section-schedule-register">
                    <div class="content-headertl">
                        <span>ĐĂNG KÍ DỰ GIỜ</span>
                    </div>
                    <div class="">
                        <div class="table-responsive bg-white table-thead-red">
                            <table class="table table-bordered mb-0">
                                <thead>
                                    <tr>
                                        <th style="width: 70px;">#</th>
                                        <th>Lịch</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="rpThuTKBDuGio" OnItemDataBound="rpThuTKBDuGio_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <th>Thứ <%# Container.DataItem  %>
                                                </th>
                                                <th>
                                                    <div class="list-register">
                                                        <asp:Repeater ID="rptTKBPCNL" runat="server">
                                                            <ItemTemplate>
                                                                <asp:UpdatePanel runat="server">
                                                                    <ContentTemplate>
                                                                        <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                                            <div class="--class"><%#Eval("lichbaogiangchitiet_lop") %></div>
                                                                            <%#Eval("lichbaogiangchitiet_monhoc") %></a>
                                                                        <%--<a href="javascript:void(0)" onclick="getId(<%#Eval("lichbaogiangchitiet_id") %>)" class="btn btn-danger btn-sm"><%--data-toggle="modal" data-target="#modalPCNL"--%>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </th>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                    <%--<tr>
                                        <th>Thứ 2
                                        </th>
                                        <td>
                                            <div class="list-register">
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.2</div>
                                                    Môn toán</a>
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.1</div>
                                                    Môn toán</a>
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.2</div>
                                                    Môn toán</a>
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.1</div>
                                                    Môn toán</a>
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.2</div>
                                                    Môn toán</a>
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.1</div>
                                                    Môn toán</a>
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.2</div>
                                                    Môn toán</a>
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.1</div>
                                                    Môn toán</a>
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.2</div>
                                                    Môn toán</a>
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.1</div>
                                                    Môn toán</a>
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.2</div>
                                                    Môn toán</a>
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.1</div>
                                                    Môn toán</a>
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.2</div>
                                                    Môn toán</a>
                                                <a href="javascript:void(0)" data-toggle="modal" data-target="#modalRegisterDuGio" class="btn btn-register btn-sm">
                                                    <div class="--class">6.1</div>
                                                    Môn toán</a>
                                            </div>
                                        </td>
                                    </tr>--%>
                                </tbody>
                            </table>

                        </div>

                    </div>
                </div>
            </div>
        </div>

    </div>
    <!-- Modal -->
    <div class="modal fade" id="modalRegisterDuGio" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">CHI TIẾT LỊCH BÁO GIẢNG</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <table class="table table-sm table-bordered">
                        <thead>
                            <tr>
                                <th>Thứ</th>
                                <th>Buổi</th>
                                <th>Tiết</th>
                                <th>Lớp</th>
                                <th>Ngày dạy</th>
                                <th>Môn</th>
                                <th>Tiết<br />
                                    (PPCT)
                                </th>
                                <th>Tên bài giảng</th>
                                <th>Ghi chú</th>
                                <th>Người soạn</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Thứ 2</td>
                                <td>Sáng</td>
                                <td>Tiết 4</td>
                                <td>1/3</td>
                                <td>04-09-2023</td>
                                <td>Tiếng Anh 1</td>
                                <td></td>
                                <td>NGHỈ LỄ</td>
                                <td></td>
                                <td>Nguyễn Thị Thúy Ngân</td>
                            </tr>

                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Đóng</button>
                    <button type="button" class="btn btn-primary">Đăng kí dự giờ</button>
                </div>
            </div>
        </div>
    </div>
    <div class="section-tasks-months">


        <%--<div class="heading-section">
                <h3 class="mb-4">THÔNG BÁO NỘI BỘ</h3>
                <a href="/admin-xem-thong-bao-noi-bo" class="btn btn-viewall">Xem tất cả</a>
            </div>
            <div id="slide-news" class="owl-carousel owl-theme">
                <asp:Repeater runat="server" ID="rpThongBao">
                    <ItemTemplate>
                        <div class="item">
                            <div class="block-news">

                                <a class="block-main" href="/admin-file-huong-dan-danh-gia-<%#Eval("thongbao_id") %>">
                                    <div class="title" title="<%#Eval("thongbao_name") %>">
                                        <%#Eval("thongbao_name") %>
                                        <span class="status" style="top: 0; <%#Eval("style")%>">
                                            <img src="/images/new-1.gif"></span>
                                    </div>
                                    <div class="summary">
                                        <i class="fa fa-university"></i>&nbsp;<%#Eval("thongbao_cap")%>
                                        <span style="float: right">
                                            <i class="fa fa-calendar"></i>&nbsp;<%#Convert.ToDateTime(Eval("thongbao_datecreate")).ToShortDateString()%>
                                        </span>
                                    </div>
                                    <div class="button">
                                        <div class="btn btn-link">Xem tiếp&nbsp;<i class="fa fa-arrow-circle-o-right"></i></div>
                                    </div>
                                </a>

                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>--%>
    </div>

    <script>
        function clickCongViec(id) {
            document.getElementById('<%=txtCongViec_id.ClientID%>').value = id;
            document.getElementById('<%=btnLuuLichSu.ClientID%>').click();
        }


        $(document).ready(function () {
            $('.notification-content').vTicker({
                speed: 500,
                pause: 4000,
                showItems: 10,
                animation: 'fade',
                mousePause: false,
                direction: 'up'
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BottomWrapper" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Footer" runat="Server">
</asp:Content>

