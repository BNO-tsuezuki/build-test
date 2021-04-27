use evogmtool;

insert into Domains (domainId, domainName) values (1, 'BNA');

insert into Regions (regionId, regionCode) values (1, 'JP');
insert into Regions (regionId, regionCode) values (2, 'NA');

insert into Publishers (publisherId, publisherName) values (1, 'BNO');
insert into Publishers (publisherId, publisherName) values (2, 'BNEA');

insert into Languages (languageCode, languageName) values ('ja', 'Japanese');
insert into Languages (languageCode, languageName) values ('en', 'English');
insert into Languages (languageCode, languageName) values ('fr', 'French');
insert into Languages (languageCode, languageName) values ('de', 'German');

insert into Timezones (timezoneCode) values ('Japan');
insert into Timezones (timezoneCode) values ('US/Alaska');
insert into Timezones (timezoneCode) values ('US/Central');
insert into Timezones (timezoneCode) values ('US/Eastern');
insert into Timezones (timezoneCode) values ('US/Hawaii');
insert into Timezones (timezoneCode) values ('US/Mountain');
insert into Timezones (timezoneCode) values ('US/Pacific');

insert into DomainRegions (domainId, regionId, publisherId, target) values (1, 1, 1, 1);
insert into DomainRegions (domainId, regionId, publisherId, target) values (1, 2, 2, 2);

insert into DomainRegionLanguages (domainId, regionId, languageCode) values (1, 1, 'ja');
insert into DomainRegionLanguages (domainId, regionId, languageCode) values (1, 2, 'en');
insert into DomainRegionLanguages (domainId, regionId, languageCode) values (1, 2, 'fr');
insert into DomainRegionLanguages (domainId, regionId, languageCode) values (1, 2, 'de');

insert into Users (userId, account, salt, passwordHash, name, role, publisherId, isAvailable, languageCode, timezoneCode) values (1, 'superuser', '5A9130EBB116A6031180536997552970', '8C0963B2CC65EC8B4C378FA3249C3FA209244AEAFE4FC1E6AA1D948C227A16F5814B44D809FD8815FC225EF4F79AB9856D93199A5F640B25560194F5A149A69D', 'superuser', 'super', 1, 1, 'ja', 'Japan');
