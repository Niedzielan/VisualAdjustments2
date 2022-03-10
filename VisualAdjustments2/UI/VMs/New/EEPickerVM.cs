﻿using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.BundlesLoading;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.ServiceWindow;
using Kingmaker.UnitLogic;
using Kingmaker.Visual.CharacterSystem;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using VisualAdjustments2.Infrastructure;

namespace VisualAdjustments2.UI
{
    // TODO: Make this check if in EEPicker or smthn
    [HarmonyLib.HarmonyPatch(typeof(Character),nameof(Character.CopyEquipmentFrom))]
    public static class asdsadasd
    {
        public static bool Prefix(Character __instance)
        {
            if (ServiceWindowsVM_ShowWindow_Patch.swPCView.m_EEPickerPCView.ViewModel != null && __instance.EquipmentEntities.Count != 0)
            {
                return false;
            }
            else return true;
        }
    }
    public class EEPickerVM : BaseDisposable, IDisposable, IViewModel, IBaseDisposable
    {
        public Dictionary<string,EEApplyAction> applyActions = new Dictionary<string, EEApplyAction>();

        public ReactiveProperty<UnitDescriptor> UnitDescriptor;
        public ReactiveProperty<ListViewVM> AllEEs = new ReactiveProperty<ListViewVM>();
        public ReactiveProperty<ListViewVM> CurrentEEs = new ReactiveProperty<ListViewVM>();
        public EEPickerVM(UnitEntityData data)
        {
            base.AddDisposable(this);
            // data.View.CharacterAvatar.EquipmentEntities.ObserveEveryValueChanged(a => a).Subscribe(a => { CurrentEEs.EntitiesCollection.Clear(); CurrentEEs.EntitiesCollection.Add(a); });
            //base.AddDisposable();
            ReactiveCollection<ListViewItemVM> reactive = new ReactiveCollection<ListViewItemVM>();
            foreach (var kv in ResourceLoader.AllEEs)
            {
                reactive.Add(new ListViewItemVM(kv, true, AddListItem));
            }
            var CurrentReactive = new ReactiveCollection<ListViewItemVM>();
            foreach (var ee in Game.Instance.SelectionCharacter.SelectedUnit.Value.Unit.View.CharacterAvatar.EquipmentEntities)
            {
                //Main.Logger.Log(ee.name);
                var inf = ee.ToEEInfo();
                if (inf != null && !CurrentReactive.Any(a => a.Guid == inf.Value.GUID))
                {
                    CurrentReactive.Add(new ListViewItemVM(inf.Value, false, RemoveListItem));
                }
            }
            this.UnitDescriptor = Game.Instance.SelectionCharacter.SelectedUnit;
            base.AddDisposable(Game.Instance.SelectionCharacter.SelectedUnit.Subscribe(delegate (UnitDescriptor _)
            {
                this.OnUnitChanged();
            }));
            // bool v = Game.Instance.SelectionCharacter.SelectedUnit.Value.Unit.View.CharacterAvatar..;
            // base.AddDisposable(v);
            base.AddDisposable(AllEEs.Value = new ListViewVM(reactive, new ReactiveProperty<ListViewItemVM>(reactive.FirstOrDefault())));
            base.AddDisposable(CurrentEEs.Value = new ListViewVM(CurrentReactive, new ReactiveProperty<ListViewItemVM>(CurrentReactive.FirstOrDefault())));
            //CurrentEEs = new ListViewVM();
        }
        private void OnUnitChanged()
        {
            // TODO: Change logic here
            {
                var CurrentReactive = new ReactiveCollection<ListViewItemVM>();
                foreach (var ee in Game.Instance.SelectionCharacter.SelectedUnit.Value.Unit.View.CharacterAvatar.EquipmentEntities)
                {
                    //Main.Logger.Log(ee.name);
                    var inf = ee.ToEEInfo();
                    if (inf != null && !CurrentReactive.Any(a => a.Guid == inf.Value.GUID))
                    {
                        CurrentReactive.Add(new ListViewItemVM(inf.Value, false, RemoveListItem));
                    }
                }
                CurrentEEs.Value?.Dispose();
                base.AddDisposable(CurrentEEs.Value = new ListViewVM(CurrentReactive, new ReactiveProperty<ListViewItemVM>(CurrentReactive.FirstOrDefault())));
            }
            if (this.UnitDescriptor.Value == null)
            {
                return;
            }
           // this.UpdateCanChangeEquipment();
            //InventoryStashVM stashVM = this.StashVM;
            //if (stashVM == null)
            {
              //  return;
            }
        }
        public void RemoveListItem(ListViewItemVM item)
        {
            try
            {
                if (this.CurrentEEs?.Value?.EntitiesCollection?.Contains(item) == true)
                { 

                    this.CurrentEEs?.Value?.EntitiesCollection.Remove(item);
                    Game.Instance.UI.Common.DollRoom.m_Avatar.RemoveEquipmentEntity(ResourcesLibrary.TryGetResource<EquipmentEntity>(item.Guid));
                    if (!this.applyActions.ContainsKey(item.Guid)) this.applyActions.Add(item.Guid,new RemoveEE(item.Guid));
                }
            }
            catch (Exception e)
            {

                Main.Logger.Error(e.ToString());
            }
        }
        public void AddListItem(ListViewItemVM item)
        {
            try
            {
                if (!this.CurrentEEs?.Value?.EntitiesCollection.Any(a => a.Guid == item.Guid) == true) this.CurrentEEs?.Value.EntitiesCollection.Add(new ListViewItemVM(item, false, RemoveListItem));
                Main.Logger.Log(ResourcesLibrary.TryGetResource<EquipmentEntity>(item.Guid).ToString());
                /*if(Game.Instance.UI.Common.DollRoom.m_Avatar.EquipmentEntities.Any(a => a.name == item.InternalName))*/ 
                Game.Instance.UI.Common.DollRoom.m_Avatar.AddEquipmentEntity(ResourcesLibrary.TryGetResource<EquipmentEntity>(item.Guid));
                if (!this.applyActions.ContainsKey(item.Guid)) this.applyActions.Add(item.Guid,new AddEE(item.Guid));
            }
            catch (Exception e)
            {

                Main.Logger.Error(e.ToString());
            }
        }
        public void ApplyColor(Color col,bool PrimOrSec)
        {
            this.Sel
        }
        public override void DisposeImplementation()
        {

        }
    }
}
