﻿using Dungeon;
using Dungeon.Localization;

namespace Nabunassar.Locale
{
    internal class GameStrings : LocalizationStringDictionary
    {
        public override string ___RelativeLocalizationFilesPath => "locale";

        public override string ___DefaultLanguageCode => "ru";

        public LocalizationStringSubDictionary Description { get; set; }
    }
}