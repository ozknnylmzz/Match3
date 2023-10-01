using CasualA.Board;

public class Constants
{
    #region PowerUps

    public const int BOMB_RADIUS = 1;

    #endregion

    #region Boosters

    public const int JESTER_COUNT = 2;

    public const int SLIME_MAX_REGION_SIZE = 11;
    public const int SLIME_MIN_REGION_SIZE = 3;

    #endregion

    #region Mixers

    #endregion

    #region Board

    public static readonly int[] CONFIGURETYPES_PIECE_VALUE_4 = new int[] { 0, 1, 2, 3, };
    public static readonly int[] CONFIGURETYPES_PIECE_VALUE_6 = new int[] { 0, 1, 2, 3, 4, 5 };

    public static readonly ItemType[] DEFAULT_ITEM_TYPES = { ItemType.BoardItem, ItemType.Bomb, ItemType.Lightball, ItemType.HorizontalRocket, ItemType.VerticalRocket }; // TODO Defaults Board Item Types

    #endregion

    #region Item Scores

    public const int BOARD_ITEM_SCORE = 1;
    public const int POWERUP_ITEM_SCORE = 1;
    public const int EGG_SCORE = 3;
    public const int LEAF_SCORE = 3;
    public const int FIREWORKS_SCORE = 0;

    #endregion

    #region Build Indexes

    public const int MAIN_SCENE_BUILD_INDEX = 1;
    public const int BOARD_SCENE_BUILD_INDEX = 2;
    public const int SINGLEPLAYER_SCENE_BUILD_INDEX = 4;

    #endregion

    #region TurnBasedGameConfig

    public const int TOTAL_ROUNDS = 5;
    public const int PLAYER_MOVE_COUNT_PER_ROUND = 2;
    public const int ROUND_TIME = 30;

    #endregion

    public const int BOOSTER_ANIMATION_PRIORITY = 0;
    public const int MIXER_PLACE_PRIORITY = 10;

    public const string FUSION_SESSION_NAME = "Anil";

    public const string DEFAULT_LAYER = "Default";
    public const string UI_PARTICLE_LAYER = "UiParticle";
}