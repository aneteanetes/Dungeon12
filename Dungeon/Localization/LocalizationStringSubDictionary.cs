﻿namespace Dungeon.Localization
{
    public class LocalizationStringSubDictionary
    {
        LocalizationStringDictionary _localizationStringDictionary;
        string _propertyName;

        public LocalizationStringSubDictionary(LocalizationStringDictionary localizationStringDictionary, string propertyName)
        {
            _localizationStringDictionary= localizationStringDictionary;
            _propertyName= propertyName;
        }

        public string this[string @const] => _localizationStringDictionary[_propertyName+"."+@const];

        public string this[object obj] => _localizationStringDictionary.GetValueEnumPrefix(_propertyName+".",obj);
    }
}