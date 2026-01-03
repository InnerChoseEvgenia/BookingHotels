using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookingHotelAPI.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.Property(q => q.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.HotelId);//поиск по айди отеля
        builder.HasIndex(x => new { x.CheckIn, x.CheckOut }); //это индексы для поска в бд, это составной индекс по занятым датам
    }
}
