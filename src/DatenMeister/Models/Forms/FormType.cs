namespace DatenMeister.Models.Forms
{
    public enum FormType
    {
        /// <summary>
        /// Set, when the form is requested for a detail form in which one object is shown
        /// in a separate window area and in which the user cannot navigate to another object easily
        /// </summary>
        Detail,

        /// <summary>
        /// Set, when the form is created when the user clicked upon an extent in an explorer view and the complete
        /// extent including its root items shall be shown
        /// </summary>
        TreeItemExtent,
        TreeItemList,

        /// <summary>
        /// Set,when the form is created when the user clicks upon on item in detail view and the form is created
        /// This flag is set when an extent form is requested for the complete detail or when a list form is requested
        /// for one property or property's metaclass of the selected element.
        /// </summary>
        TreeItemDetail,

        /// <summary>
        /// Set, when a object's property is listed in an enumeration as given for a sub elements view for example.
        /// </summary>
        ObjectList
    }
}