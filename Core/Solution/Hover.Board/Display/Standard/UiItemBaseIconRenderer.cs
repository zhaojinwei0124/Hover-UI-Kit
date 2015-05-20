using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.Display;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public abstract class UiItemBaseIconRenderer : UiItemSelectRenderer {

		protected GameObject vIcon;
		protected Mesh vIconMesh;

		private int vPrevTextSize;
		private bool vIsSizeChanged;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract Materials.IconOffset GetIconOffset();

		/*--------------------------------------------------------------------------------------------*/
		protected virtual Vector3 GetIconScale() {
			float s = vSettings.TextSize*0.75f*LabelCanvasScale;
			return new Vector3(s, s, 1);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(IHoverboardPanelState pPanelState,
										IHoverboardLayoutState pLayoutState, IBaseItemState pItemState,
										IItemVisualSettings pSettings) {
			base.Build(pPanelState, pLayoutState, pItemState, pSettings);

			vLabel.AlignLeft = true;

			////

			vIcon = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vIcon.name = "Icon";
			vIcon.transform.SetParent(gameObject.transform, false);
			vIcon.transform.localRotation = 
				vLabel.gameObject.transform.localRotation*vLabel.CanvasLocalRotation;
			vIcon.transform.localScale = GetIconScale();

			vIconMesh = vIcon.GetComponent<MeshFilter>().mesh;
			Materials.SetMeshColor(vIconMesh, Color.clear);
			Materials.SetMeshIconCoords(vIconMesh, GetIconOffset());
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void SetDepthHint(int pDepthHint) {
			base.SetDepthHint(pDepthHint);

			vIcon.GetComponent<MeshRenderer>().sharedMaterial = 
				Materials.GetLayer(Materials.Layer.Icon, pDepthHint, "StandardIcons");
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void SetCustomSize(float pWidth, float pHeight, bool pCentered=true) {
			base.SetCustomSize(pWidth, pHeight, pCentered);
			vLabel.transform.localPosition = new Vector3(-vWidth/2, 0, 0);
			vIsSizeChanged = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();

			Color color = vSettings.ArrowIconColor;
			color.a *= (vItemState.MaxHighlightProgress*0.75f + 0.25f)*vMainAlpha;

			Materials.SetMeshColor(vIconMesh, color);

			if ( vSettings.TextSize != vPrevTextSize || vIsSizeChanged ) {
				vPrevTextSize = vSettings.TextSize;
				vIsSizeChanged = false;

				float inset = vSettings.TextSize;
				vLabel.SetInset(false, inset);

				vIcon.transform.localPosition = new Vector3(
					vWidth/UiItem.Size/2-inset*0.666f*LabelCanvasScale, 0, 0);
			}
		}

	}

}
