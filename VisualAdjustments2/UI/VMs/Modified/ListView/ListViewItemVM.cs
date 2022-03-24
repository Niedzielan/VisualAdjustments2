﻿using JetBrains.Annotations;
using Kingmaker.UI.MVVM._VM.Other.NestedSelectionGroup;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.Tooltip;
using Owlcat.Runtime.UI.SelectionGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using VisualAdjustments2;

namespace VisualAdjustments2.UI
{
    public class ListViewItemVM : SelectionGroupEntityVM
    {
		public const string Add = "<";
		public const string Remove = ">";
		public ListViewItemVM(ResourceInfo ee,bool addorremove,Action<ListViewItemVM> OnSelectAction) : base(false)
		{
			this.Guid = ee.GUID;
			this.DisplayName = ee.Name;
			this.InternalName = ee.Name_Internal;
			this.AddOrRemove = addorremove;
			this.action = OnSelectAction;
			base.AddDisposable(this);
		}
		public ListViewItemVM(ListViewItemVM ToClone, bool addorremove, Action<ListViewItemVM> OnSelectAction) : base(false)
		{
			this.Guid = ToClone.Guid;
			this.DisplayName = ToClone.DisplayName;
			this.InternalName = ToClone.InternalName;
			this.AddOrRemove = addorremove;
			this.action = OnSelectAction;
			base.AddDisposable(this);
		}
		public bool HasText(string searchRequest)
		{
			return (this.InternalName != null && this.DisplayName != null) && (this.InternalName.IndexOf(searchRequest, StringComparison.InvariantCultureIgnoreCase) >= 0 || this.DisplayName.IndexOf(searchRequest, StringComparison.InvariantCultureIgnoreCase) >= 0);
		}
		public override void DoSelectMe()
		{
			//action(this);
		}
		public override void DisposeImplementation()
		{
			base.DisposeImplementation();
		}
		public ReactiveProperty<TooltipBaseTemplate> m_TooltipTemplate;
		public readonly string DisplayName;
		public readonly string InternalName;
		public readonly string Guid;
		public bool AddOrRemove;
		public Action<ListViewItemVM> action;
	}
}
