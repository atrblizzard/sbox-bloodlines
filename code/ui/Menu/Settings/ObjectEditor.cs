using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;
using Sandbox.UI.Construct;

namespace Bloodlines.Menu;

public class ObjectEditor : Panel
{
	private const string EmptyGroupName = "#GameSettings.Group.Misc";

	public ObjectEditor()
	{
		Style.FlexDirection = FlexDirection.Column;
	}

    protected List<string> GroupOrder = new List<string>()
    {
        SocialGroup,
        CombatGroup,
        SoundGroup,
        ClassGroup,
        OtherGroup
    };

    public const string SocialGroup = "#GameSettings.Group.Social";
    public const string CombatGroup = "#GameSettings.Group.Combat";
    public const string ClassGroup = "#GameSettings.Group.Class";
    public const string SoundGroup = "#GameSettings.Group.Sound";
    public const string OtherGroup = "#GameSettings.Group.Misc";


    public int GetGroupOrder(string group)
    {
        int index = GroupOrder.FindIndex(x => x == group);
        if (index < 0) index = int.MaxValue;
        return index;
    }

    public void SetTarget( object target )
	{
		DeleteChildren( true );

		var properties = TypeLibrary.GetPropertyDescriptions( target );

		//Sort all properties based on Display Info Group value
		Dictionary<string, List<PropertyDescription>> groupedProperties = new Dictionary<string, List<PropertyDescription>>();
		foreach ( var property in properties )
		{
			var displayInfo = property.GetDisplayInfo();

			if (!property.IsStatic)
			{
				//Use default group if empty
				string group = string.IsNullOrEmpty(displayInfo.Group) ? EmptyGroupName : displayInfo.Group;

				if (!groupedProperties.TryGetValue(group, out var groupPropertyList))
				{
					groupPropertyList = new List<PropertyDescription>();
					groupedProperties.Add(group, groupPropertyList);
				}

				groupPropertyList.Add(property);
			}
		}

		//Sort the grouped properties
		var groups = new List<KeyValuePair<string, List<PropertyDescription>>>(groupedProperties);
		groups.Sort((a, b) =>
		{
			int orderA = GetGroupOrder(a.Key);
			int orderB = GetGroupOrder(b.Key);

			return orderA.CompareTo(orderB);
        });

		//Loop over all groups
		foreach ( var propertyGroup in groups )
		{
			Panel group = Add.Panel( "group" );
			//Make heading
			Label header = group.Add.Label(classname: "header");

			header.SetContent(propertyGroup.Key);

			//Add settings row for properties
			foreach(var groupProperty in propertyGroup.Value)
			{
                group.AddChild(new SettingRow(target, groupProperty) { Classes = "item" } );
            }
        }
	}
}
