using SalesApp.Interfaces;
using SalesApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Data {
    class SalesContext : DbContext {

        //Create Entity Context Objects
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SalesPerson> People { get; set; }
        public DbSet<SalesRegion> Regions { get; set; }

        //Overrride DB default model creation
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            //Remove default conventions
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();//stops Cascading deletion if an entity has no more ties to other entities
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//removes 's' added to the end of tables created from classes

            //You can also add specific table features/requirements here instead of annotation
            //modelBuilder.Entity<Sale>().HasRequired(e => e.Person);
            //modelBuilder.Entity<Sale>().HasOptional(e => e.Region);
            //modelBuilder.Entity<Sale>().HasKey(e => e.Id);
        }

        //Overrides SaveChanges() behavior for soft deletion and auditing information
        public override int SaveChanges() {
            //make sure all changes are detected before doing override tasks
            ChangeTracker.DetectChanges();
            
            //ObjectStateManager tracks all entities that are currently being tracked by the DB Context
            var stateManager = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager;
            

            //SOFT DELETE MODIFICATION//
            //Instead of deleting objects, we want to mark them as inactive, this gets the deleted objects
            var deletedEntities = stateManager
                .GetObjectStateEntries(EntityState.Deleted)//any entities that have been deleted
                .Select(e=> e.Entity)//select that entity
                .OfType<IActive>()//if it is of Type IActive
                .ToArray();//Send Entity to an array
            
            //iterate through all entities, mark as inactive instead of deleting
            foreach (var deletedEntity in deletedEntities) {
                //delete if null
                if (deletedEntity == null) continue;
                //Don't delete and mark modified, then modification auditing below will also kick in
                //Modification auditing code should be below this one so that objects marked as 
                //modified here, will be picked up by the below code
                stateManager.ChangeObjectState(deletedEntity,EntityState.Modified);
                //Set entity to inactive
                deletedEntity.Active = false;
                   
            }

            //CREATION AUDITING MODIFICATION//
            //This will ensure that newly created objects save creation time and who created it
            var createdEntities = stateManager
                .GetObjectStateEntries(EntityState.Added)//any entities that have been created
                .Select(e => e.Entity)//select that entity
                .OfType<BaseModel>()//if it is of Type BaseModel
                .ToArray();//Send Entity to an array

            //loop through entities and save CreatedDate and CreatedBy poperties
            foreach (var createdEntity in createdEntities) {
                //save creation time
                createdEntity.CreatedDate = DateTime.Now;
                //save user who created entity
                createdEntity.CreatedBy = Environment.UserName;
                //Updated fields are also required so we need to set them as well on new entities
                createdEntity.UpdatedDate = DateTime.Now;
                createdEntity.UpdatedBy = Environment.UserName;

            }

            //UPDATE AUDITING MODIFICATION//
            //This will ensure that modified objects save update time and who updated it
            var updatedEntities = stateManager
                .GetObjectStateEntries(EntityState.Modified)//any entities that have been modified
                .Select(e => e.Entity)//select that entity
                .OfType<BaseModel>()//if it is of Type BaseModel
                .ToArray();//Send Entity to an array

            //loop through entities and save CreatedDate and CreatedBy poperties
            foreach (var updatedEntity in updatedEntities) {
                //save update time and user
                updatedEntity.UpdatedDate = DateTime.Now;
                updatedEntity.UpdatedBy = Environment.UserName;

            }

            return base.SaveChanges();
        }
    }
}
