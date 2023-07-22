using ServiceObjects;
using WithMVCS;
using Zenject;

namespace Chess_AI
{
    public class AIInstaller : Installer<AIInstaller>
    {
        private LvlData _lvlData;

        public AIInstaller(LvlData lvlData)
        {
            _lvlData = lvlData;
        }
        public override void InstallBindings()
        {
            Container.BindFactory<PieceType, int, PieceColor, (int, int), AIPiece, AIPiece.Factory>().AsSingle().NonLazy();
            Container.BindMemoryPool<AITurn, AITurn.Pool>().WithInitialSize(150).NonLazy();
            Container.Bind<MinMaxSystem>().AsSingle();
            Container.Bind<AIPiece[][]>().FromMethod(SetAIBoard).AsSingle();
            Container.BindInterfacesAndSelfTo<AIChessBoard>().AsSingle()
                .WithArguments(_lvlData.playerType == PieceColor.Black ? PieceColor.White : PieceColor.Black);
        }
        private AIPiece[][] SetAIBoard()
        {
            return new[]
            {
                new AIPiece[8], new AIPiece[8], new AIPiece[8], new AIPiece[8], new AIPiece[8],
                new AIPiece[8], new AIPiece[8], new AIPiece[8]
            };
        }
    }
}