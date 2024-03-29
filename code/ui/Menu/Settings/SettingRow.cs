﻿
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;

namespace Bloodlines.Menu;

internal class SettingRow : Panel
{
	public Label Label { get; }
	public Panel ValueArea { get; }

	public SettingRow( object target, PropertyDescription property ) : this()
	{
		Label.Text = property.GetDisplayInfo().Name;

		var typeDesc = TypeLibrary.GetType( property.PropertyType );
		var currentValue = property.GetValue( target );

		if ( property.PropertyType == typeof( bool ) )
		{
			var checkbox = new Checkbox { Parent = ValueArea };
			checkbox.ValueChanged += ( bool c ) =>
			{
				property.SetValue( target, c );
				CreateEvent( "save" );
			};
			
		}
		else if ( property.PropertyType == typeof( string ) )
		{
			var textentry = ValueArea.Add.TextEntry( (string)currentValue );
			textentry.AddEventListener( "value.changed", () =>
			{
				property.SetValue( target, textentry.Value );
				CreateEvent( "save" );
			} );
		}

		if ( property.PropertyType.IsEnum )
		{
			var dropdown = new DropDown( ValueArea );
			dropdown.SetPropertyObject( "value", currentValue );
			dropdown.ValueChanged += (string value) =>
			{
				Enum.TryParse( property.PropertyType, value, out var newval );
				property.SetValue( target, newval );
				CreateEvent( "save" );
			};
		}
		else if ( property.PropertyType == typeof( float ) )
		{
			var minmax = property.GetCustomAttribute<MinMaxAttribute>();
			var min = minmax?.MinValue ?? 0f;
			var max = minmax?.MaxValue ?? 100f;
			var step = property.GetCustomAttribute<SliderStepAttribute>()?.Step ?? .1f;
			
			var slider = new SliderControl( min, max, step );
			slider.Parent = ValueArea;
			slider.Bind( "value", target, property.Name );
			slider.OnValueChanged += (float _) =>CreateEvent( "save" );
		}
		else if ( property.PropertyType == typeof( int ) )
		{
			var minmax = property.GetCustomAttribute<MinMaxAttribute>();
			var min = minmax?.MinValue ?? 0;
			var max = minmax?.MaxValue ?? 100;
			var step = property.GetCustomAttribute<SliderStepAttribute>()?.Step ?? 1;
			var slider = new SliderControl( min, max, step );
			slider.Parent = ValueArea;
			slider.Bind( "value", target, property.Name );
			slider.OnValueChanged += ( float _ ) => CreateEvent( "save" );
		}
	}

	public SettingRow()
	{
		Label = Add.Label( "Label" );
		Add.Panel().Style.FlexGrow = 1;
		ValueArea = Add.Panel( "value-area" );
	}

}
