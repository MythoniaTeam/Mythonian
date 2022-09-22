



namespace Mythonia.Game.Sprites.Effects
{
    public class AimingLineVertical : Sprite
    {
        public AimingLineVertical(MGame game, Map map) 
            : base("AimingLineVertical", game, map,
            MTextureManager.Ins["AimingLineVertical"].PlayAnimation())
        {

        }

        public AnimationPlayer AnimaitonPlayer => (AnimationPlayer)Texture;
        public void Activate()
        {
            AnimaitonPlayer.PlayAnimation("Activate", false);
            AnimaitonPlayer.OnFinishPlaying += AnimaitonPlayer_OnFinishPlaying;
        }

        private void AnimaitonPlayer_OnFinishPlaying(AnimationMeta current)
        {
            MGame.Components.Remove(this);
        }
    }
}
