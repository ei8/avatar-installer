using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Cortex.Coding;
using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Persistence;
using ei8.Cortex.Coding.Persistence;
using ei8.Cortex.Coding.Reflection;
using ei8.EventSourcing.Client;
using neurUL.Common.Domain.Model;

namespace ei8.Avatar.Installer.IO.Process.Services.Avatars
{
    public class AvatarWriteRepository : IAvatarWriteRepository
    {
        private readonly ITransaction transaction;
        private readonly INetworkTransactionData networkTransactionData;
        private readonly INetworkTransactionService networkTransactionService;
        private readonly IneurULizer neurULizer;
        private readonly IMirrorRepository mirrorRepository;

        public AvatarWriteRepository(
            ITransaction transaction,
            INetworkTransactionData networkTransactionData,
            INetworkTransactionService networkTransactionService,
            IneurULizer neurULizer,
            IMirrorRepository mirrorRepository
        )
        {
            AssertionConcern.AssertArgumentNotNull(transaction, nameof(transaction));
            AssertionConcern.AssertArgumentNotNull(networkTransactionData, nameof(networkTransactionData));
            AssertionConcern.AssertArgumentNotNull(networkTransactionService, nameof(networkTransactionService));
            AssertionConcern.AssertArgumentNotNull(neurULizer, nameof(neurULizer));
            AssertionConcern.AssertArgumentNotNull(mirrorRepository, nameof(mirrorRepository));

            this.transaction = transaction;
            this.networkTransactionData = networkTransactionData;
            this.networkTransactionService = networkTransactionService;
            this.neurULizer = neurULizer;
            this.mirrorRepository = mirrorRepository;
        }

        public async Task Save(Domain.Model.Avatars.Avatar avatar, CancellationToken token = default)
        {
            // TODO:1 handle updates - message.Version == 0 ? WriteMode.Create : WriteMode.Update

            var typeInfo = neurULizerTypeInfo.GetTypeInfo(avatar);

            // use key to retrieve external reference url from library
            var mirrors = await this.mirrorRepository.GetByKeysAsync(
                typeInfo.Keys.Except(new[] { string.Empty }).ToArray()
            );

            var valueNeuronIds = typeInfo.GrannyProperties
                .Where(gp => gp.ValueMatchBy == ValueMatchBy.Id)
                .Select(gp => gp.Value);

            var idPropertyValueNeurons = this.networkTransactionData.SavedTransientNeurons;

            var nn = neurULizer.neurULize(
                avatar,
                idPropertyValueNeurons,
                typeInfo,
                mirrors
            );

            await this.networkTransactionService.SaveAsync(this.transaction, nn);
        }
    }
}
