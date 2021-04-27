using System;
using System.Linq;
using System.Threading.Tasks;
using evotool.ProtocolModels.GMTool.VersionApi;
using Microsoft.EntityFrameworkCore;

namespace evotool.Services.GMTool
{
    public interface IVersionService
    {
        Task<GetVersionResponse> GetVersion();
        Task<PutVersionResponse> PutVersion(PutVersionRequest request);
    }

    public class VersionService : BaseService, IVersionService
    {
        public VersionService(IServicePack servicePack) : base(servicePack)
        { }

        public async Task<GetVersionResponse> GetVersion()
        {
            var response = new GetVersionResponse();

            var enabledVersions = await Common1DB.EnabledVersions.ToListAsync();

            var loginVersion = enabledVersions.SingleOrDefault(r => r.checkTarget == evolib.VersionChecker.CheckTarget.Login &&
                                                                    r.referenceSrc == evolib.VersionChecker.ReferenceSrc.PackageVersion);

            if (loginVersion == null)
            {
                // todo: error message
                throw new Exception("login version not registered");
            }

            response.loginVersion = new GetVersionResponse.LoginVersion
            {
                packageVersion = BuildPackageVersion(loginVersion),
            };

            var matchmakeVersion = enabledVersions.SingleOrDefault(r => r.checkTarget == evolib.VersionChecker.CheckTarget.Matchmake &&
                                                                        r.referenceSrc == evolib.VersionChecker.ReferenceSrc.PackageVersion);

            if (matchmakeVersion == null)
            {
                // todo: error message
                throw new Exception("matchmake version not registered");
            }

            response.matchmakeVersion = new GetVersionResponse.MatchmakeVersion
            {
                packageVersion = BuildPackageVersion(matchmakeVersion),
            };

            var replayPackageVersion = enabledVersions.SingleOrDefault(r => r.checkTarget == evolib.VersionChecker.CheckTarget.Replay &&
                                                                            r.referenceSrc == evolib.VersionChecker.ReferenceSrc.PackageVersion);

            if (replayPackageVersion == null)
            {
                // todo: error message
                throw new Exception("replay package version not registered");
            }

            var replayMasterDataVersion = enabledVersions.SingleOrDefault(r => r.checkTarget == evolib.VersionChecker.CheckTarget.Replay &&
                                                                               r.referenceSrc == evolib.VersionChecker.ReferenceSrc.MasterDataVersion);
            if (replayMasterDataVersion == null)
            {
                // todo: error message
                throw new Exception("replay master data version not registered");
            }

            response.replayVersion = new GetVersionResponse.ReplayVersion
            {
                packageVersion = BuildPackageVersion(replayPackageVersion),
                masterDataVersion = BuildMasterDataVersion(replayMasterDataVersion),
            };

            return response;
        }

        public async Task<PutVersionResponse> PutVersion(PutVersionRequest request)
        {
            var response = new PutVersionResponse();

            var enabledVersions = await Common1DB.EnabledVersions.ToListAsync();

            var loginVersion = enabledVersions.SingleOrDefault(r => r.checkTarget == evolib.VersionChecker.CheckTarget.Login &&
                                                                    r.referenceSrc == evolib.VersionChecker.ReferenceSrc.PackageVersion);

            if (loginVersion == null)
            {
                // todo: error message
                throw new Exception("login version not registered");
            }

            UpdatePackageVersion(loginVersion, request.loginVersion.packageVersion);

            var matchmakeVersion = enabledVersions.SingleOrDefault(r => r.checkTarget == evolib.VersionChecker.CheckTarget.Matchmake &&
                                                                        r.referenceSrc == evolib.VersionChecker.ReferenceSrc.PackageVersion);

            if (matchmakeVersion == null)
            {
                // todo: error message
                throw new Exception("matchmake version not registered");
            }

            UpdatePackageVersion(matchmakeVersion, request.matchmakeVersion.packageVersion);

            var replayPackageVersion = enabledVersions.SingleOrDefault(r => r.checkTarget == evolib.VersionChecker.CheckTarget.Replay &&
                                                                            r.referenceSrc == evolib.VersionChecker.ReferenceSrc.PackageVersion);

            if (replayPackageVersion == null)
            {
                // todo: error message
                throw new Exception("replay package version not registered");
            }

            UpdatePackageVersion(replayPackageVersion, request.replayVersion.packageVersion);

            var replayMasterDataVersion = enabledVersions.SingleOrDefault(r => r.checkTarget == evolib.VersionChecker.CheckTarget.Replay &&
                                                                               r.referenceSrc == evolib.VersionChecker.ReferenceSrc.MasterDataVersion);
            if (replayMasterDataVersion == null)
            {
                // todo: error message
                throw new Exception("replay master data version not registered");
            }

            UpdateMasterDataVersion(replayMasterDataVersion, request.replayVersion.masterDataVersion);

            await Common1DB.SaveChangesAsync();

            return response;
        }

        private GetVersionResponse.PackageVersion BuildPackageVersion(evolib.Databases.common1.EnabledVersion enabledVersion)
        {
            return new GetVersionResponse.PackageVersion
            {
                major = enabledVersion.major,
                minor = enabledVersion.minor,
                patch = enabledVersion.patch,
                build = enabledVersion.build,
            };
        }

        private GetVersionResponse.MasterDataVersion BuildMasterDataVersion(evolib.Databases.common1.EnabledVersion enabledVersion)
        {
            return new GetVersionResponse.MasterDataVersion
            {
                major = enabledVersion.major,
                minor = enabledVersion.minor,
                patch = enabledVersion.patch,
            };
        }

        private void UpdatePackageVersion(evolib.Databases.common1.EnabledVersion registered, PutVersionRequest.PackageVersion requested)
        {
            registered.major = requested.major;
            registered.minor = requested.minor;
            registered.patch = requested.patch;
            registered.build = requested.build;
        }

        private void UpdateMasterDataVersion(evolib.Databases.common1.EnabledVersion registered, PutVersionRequest.MasterDataVersion requested)
        {
            registered.major = requested.major;
            registered.minor = requested.minor;
            registered.patch = requested.patch;
        }
    }
}
