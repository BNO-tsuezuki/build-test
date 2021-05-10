namespace evoapi.ProtocolModels
{
	public enum ProtocolCode
	{
		achievement_getlist = 0x0100,
		achievement_getstatus,
		achievement_savestatus,


		auth_login = 0x0200,
		auth_logout,


		battlepass_getpassstatus = 0x0300,
		battlepass_passexpsave,


		careerrecord_getpasturl = 0x0400,
		careerrecord_getself,
		careerrecord_getsocial,
		careerrecord_save,


		chat_say = 0x0500,
		chat_whisper,


		handshake = 0x0600,


		item_getinventory = 0x0700,
		item_watched2,


		masterdata_get = 0x0800,


		matching_cancelplayer = 0x0900,
		matching_deletelastbattle,
		matching_entrybattleserver,
		matching_entryplayer,
		matching_missing,//matching_getbattleslist,
		matching_reportacceptplayer,
		matching_reportbattlephase,
		matching_reportdisconnectplayer,
		matching_requestleavebattle,
		matching_searchencryptionkey,
		matching_pingattemptlist,


		matchresult_reportmatchresult = 0x0a00,


		opsnotice_getnotices = 0x0b00,


		option_getoptions = 0x0c00,
		option_setappoption,
		option_setmobilesuitoptions,


		ownmobilesuitsetting_getsettingslist = 0x0d00,
		ownmobilesuitsetting_setbodyskin,
		ownmobilesuitsetting_setemotionslot,
		ownmobilesuitsetting_setmvpcelebration,
		ownmobilesuitsetting_setornament,
		ownmobilesuitsetting_setstampslot,
		ownmobilesuitsetting_setvoicepack,
		ownmobilesuitsetting_setweaponskin,


		playerinformation_basic = 0x0e00,
		playerinformation_changeplayername,
		playerinformation_changepretendoffline,
		playerinformation_detail,
		playerinformation_playernamevalidateattribute,
		playerinformation_reportbattleresult,
		playerinformation_self,
		playerinformation_setfirstonetime,
		playerinformation_setopentype,
		playerinformation_setplayericon,
		playerinformation_tutorialend,
        playerinformation_tutorialprogress,


        premadegroup_kick = 0x0f00,
		premadegroup_leave,
		premadegroup_responseinvitation,
		premadegroup_sendinvitation,
		premadegroup_transferhost,


		social_favoritefriend = 0x1000,
		social_getfriends,
		social_getrecentplayers,
		social_muteplayer,
		social_reportonlinestate,
		social_responsefriendrequest,
		social_rupturefriend,
		social_searchfriend,
		social_sendfriendrequest,
		social_viewedfriendrequestspage,


		viewmatch_addviewnum = 0x1100,
		viewmatch_getreplayinfo,
		viewmatch_replayinfosave,
		viewmatch_searchallreplay,
		viewmatch_searchfeaturedreplay,
		viewmatch_searchrankreplay,
		viewmatch_searchuserreplay,


		supportdesk_geturl = 0x1200,


		test_dummyerror = 0x1300,
		test_tssversion,
		test_evocoinbalance,
		test_purchasesinevocoin,
		test_xybczqwxqvrdubuoku5fskyfmilut8iy,
        test_giveassets,
        test_givefreeevocoin,


        supplypod_getsupplypodstatus = 0x1400,
		supplypod_playsupplypod,


		assets_getinventory = 0x1500,


        playerreports_reportplayer = 0x1600,


        vivox_getvivoxinfo = 0x1700,
        vivox_getvivoxlogintoken,
        vivox_getvivoxjointoken,


        presentbox_getlist = 0x1800,
        presentbox_givepresent,


        givenhistory_getlist = 0x1900,

        challenge_getlist = 0x2000,
    }
}
