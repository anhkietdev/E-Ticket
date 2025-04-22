import React from 'react';
import { format } from 'date-fns';
import { vi } from 'date-fns/locale';

const TicketSummary = ({ ticketInfo }) => {
  if (!ticketInfo) {
    return <div>Không có thông tin vé</div>;
  }

  // Format datetime
  const formatDateTime = (dateTimeStr) => {
    try {
      const date = new Date(dateTimeStr);
      if (isNaN(date.getTime())) {
        // Nếu chuỗi không phải là định dạng ngày tháng hợp lệ, trả về nguyên bản
        return dateTimeStr;
      }
      return format(date, 'HH:mm - EEEE, dd/MM/yyyy', { locale: vi });
    } catch (error) {
      console.error('Error formatting date:', error);
      return dateTimeStr;
    }
  };

  // Format currency
  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('vi-VN', {
      style: 'currency',
      currency: 'VND',
      minimumFractionDigits: 0
    }).format(amount);
  };

  return (
    <div className="bg-white shadow-lg rounded-lg overflow-hidden">
      {/* Header với tiêu đề phim */}
      <div className="bg-gradient-to-r from-red-600 to-red-800 py-4 px-6">
        <h2 className="text-xl font-bold text-white">Chi tiết đặt vé</h2>
      </div>

      {/* Thông tin phim */}
      <div className="flex border-b border-gray-200">
        {/* Poster phim */}
        <div className="w-1/3 p-4">
          <img 
            src={ticketInfo.posterUrl || '/images/default-poster.jpg'} 
            alt={ticketInfo.movieTitle}
            className="w-full h-auto rounded" 
          />
        </div>

        {/* Thông tin cơ bản */}
        <div className="w-2/3 p-4">
          <h3 className="text-xl font-bold mb-2">{ticketInfo.movieTitle}</h3>
          <div className="text-sm text-gray-600 mb-1">
            <span className="font-semibold">Suất chiếu:</span> {formatDateTime(ticketInfo.showTime)}
          </div>
          <div className="text-sm text-gray-600 mb-1">
            <span className="font-semibold">Phòng chiếu:</span> {ticketInfo.room}
          </div>
          <div className="text-sm text-gray-600">
            <span className="font-semibold">Ghế:</span> {ticketInfo.seats.join(', ')}
          </div>
        </div>
      </div>

      {/* Thông tin chi tiết giá vé */}
      <div className="p-4 border-b border-gray-200">
        <h3 className="font-semibold text-lg mb-2">Chi tiết giá</h3>
        
        <div className="space-y-2">
          <div className="flex justify-between">
            <span>Giá vé ({ticketInfo.seats.length} vé)</span>
            <span>{formatCurrency(ticketInfo.pricePerSeat)} x {ticketInfo.seats.length}</span>
          </div>
          
          {ticketInfo.discount && (
            <div className="flex justify-between text-green-600">
              <span>Giảm giá</span>
              <span>- {formatCurrency(ticketInfo.discount)}</span>
            </div>
          )}
          
          {ticketInfo.serviceFee && (
            <div className="flex justify-between">
              <span>Phí dịch vụ</span>
              <span>{formatCurrency(ticketInfo.serviceFee)}</span>
            </div>
          )}
        </div>
      </div>

      {/* Tổng cộng */}
      <div className="p-4 bg-gray-50">
        <div className="flex justify-between font-bold text-lg">
          <span>Tổng cộng</span>
          <span className="text-red-600">{formatCurrency(ticketInfo.totalAmount)}</span>
        </div>
      </div>

      {/* Chú thích */}
      <div className="p-4 text-xs text-gray-500">
        <p>* Vé đã mua không thể đổi hoặc hoàn tiền</p>
        <p>* Vui lòng đến trước giờ chiếu 15-30 phút để check-in</p>
      </div>
    </div>
  );
};

export default TicketSummary;