#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

namespace DG.Data.Model
{
    public class GenericDataRepositoryLanguageBase : IGenericDataRepositoryLanguage
    {
        public string foreingKeyErrorRaised = "Foreing keys constraint raised on: \"{0}\"";
    }
}