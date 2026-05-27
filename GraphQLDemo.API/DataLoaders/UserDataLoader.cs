using FirebaseAdmin;
using FirebaseAdmin.Auth;
using GraphQLDemo.API.Schema.Queries;

namespace GraphQLDemo.API.DataLoaders
{
    public class UserDataLoader : BatchDataLoader<string, UserType>
    {
        private const int MAX_FIREBASE_BATCH_SIZE = 100;
        private readonly FirebaseAuth _firebaseAuth;
        public UserDataLoader(IBatchScheduler batchScheduler, FirebaseApp firebaseApp)
            : base(batchScheduler, new DataLoaderOptions()
            {
                MaxBatchSize = MAX_FIREBASE_BATCH_SIZE
            })
        {
            _firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);
        }

        protected override async Task<IReadOnlyDictionary<string, UserType>> LoadBatchAsync(IReadOnlyList<string> userIds, CancellationToken cancellationToken)
        {
            var userIdentifiers = userIds.Select(u => new UidIdentifier(u)).ToList();

            var returnUsers = await _firebaseAuth.GetUsersAsync(userIdentifiers);

            var users = returnUsers.Users.Select(u => new UserType
                 {
                 Id = u.Uid,
                 UserName = u.DisplayName,
                 PhotoUrl = u.PhotoUrl
            });

            return users.ToDictionary(u => u.Id);
        }
    }
}
