using Album.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Album.Configurations
{
  
        
        public class RegisterGradeConfigurations : IEntityTypeConfiguration<RegisterGrade>
        {
            public void Configure(EntityTypeBuilder<RegisterGrade> builder)
            {
            builder.HasKey(t => new { t.UserId, t.GradeId,t.EventId });
            builder.HasOne(t => t.GradeManagementt).WithMany(ur => ur.RegisterGrade)
                    .HasForeignKey(pc => pc.GradeId);
            builder.HasOne(t => t.Article).WithMany(ur => ur.RegisterGrade)
                    .HasForeignKey(pc => pc.EventId);
            builder.HasOne(t => t.AppUser).WithMany(ur => ur.RegisterGrade)
                    .HasForeignKey(pc => pc.UserId);
        }
        }
    
    
}
