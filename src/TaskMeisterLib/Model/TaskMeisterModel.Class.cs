using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;

// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.1.0.0
namespace TaskMeister.Model
{
    public class _TaskMeisterModel
    {
        public class _IActivity
        {
            public static string @Name = "Name";
            public IElement _Name = null;

            public static string @Description = "Description";
            public IElement _Description = null;

            public static string @State = "State";
            public IElement _State = null;

            public static string @Created = "Created";
            public IElement _Created = null;

            public static string @StartDate = "StartDate";
            public IElement _StartDate = null;

            public static string @FinishDate = "FinishDate";
            public IElement _FinishDate = null;

            public static string @EstmatedDuration = "EstmatedDuration";
            public IElement _EstmatedDuration = null;

        }

        public _IActivity @IActivity = new _IActivity();
        public IElement @__IActivity = new InMemoryElement();

        public class _IPerson
        {
            public static string @name = "name";
            public IElement _name = null;

            public static string @prename = "prename";
            public IElement _prename = null;

            public static string @department = "department";
            public IElement _department = null;

            public static string @role = "role";
            public IElement _role = null;

        }

        public _IPerson @IPerson = new _IPerson();
        public IElement @__IPerson = new InMemoryElement();

        public static _TaskMeisterModel TheOne = new _TaskMeisterModel();

    }

}
