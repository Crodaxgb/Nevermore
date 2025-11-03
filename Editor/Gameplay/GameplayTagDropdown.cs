using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace NevermoreStudios.Editor
{
    class GameplayTagDropdownItem : AdvancedDropdownItem
    {
        public string FullTag { get; set; }

        public GameplayTagDropdownItem(string name, string fullTag = null) : base(name)
        {
            FullTag = fullTag;
        }
    }

    class GameplayTagDropdown : AdvancedDropdown
    {
        private readonly List<string> _gameplayTags;
        private readonly System.Action<string> _onSelected;

        public GameplayTagDropdown(List<string> gameplayTags, System.Action<string> onSelected)
            : base(new AdvancedDropdownState())
        {
            _gameplayTags = gameplayTags;
            _onSelected = onSelected;
            minimumSize = new Vector2(200, 300);
        }
        
        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new GameplayTagDropdownItem("Gameplay Tags");

            foreach (string gameplayTag in _gameplayTags)
            {
                string[] tagParts = gameplayTag.Split('.');
                string cumulativePath = "";
                GameplayTagDropdownItem parent = (GameplayTagDropdownItem)root;

                foreach (string part in tagParts)
                {
                    cumulativePath = string.IsNullOrEmpty(cumulativePath) ? part : $"{cumulativePath}.{part}";

                    var child = parent.children.FirstOrDefault(childDropdown => childDropdown.name == part) as GameplayTagDropdownItem;
                    if (child == null)
                    {
                        child = new GameplayTagDropdownItem(part, cumulativePath);
                        parent.AddChild(child);
                        //If not a leaf node add the self tag as selectable
                        if (part != tagParts.Last())
                        {
                            parent.AddChild(new GameplayTagDropdownItem(part, cumulativePath));
                        }
                    }
                    child.FullTag = cumulativePath;

                    parent = child;
                }
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (item is GameplayTagDropdownItem tagItem && !string.IsNullOrEmpty(tagItem.FullTag))
            {
                _onSelected(tagItem.FullTag);
            }
        }
    }
}
