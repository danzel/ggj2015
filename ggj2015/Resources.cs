using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ggj2015
{
	static class Resources
	{
		public static Texture2D Test;

		public static void Load(ContentManager content)
		{
			Test = content.Load<Texture2D>("test");
		}
	}
}
