using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.Forms;

/// <summary>
/// Defines the result of the form creation.
/// It contains the created form, but also additional information whether
/// the factory has managed the request or finalized the request
/// </summary>
public abstract class FormCreationResult
{
    /// <summary>
    /// Gets or sets a value whether the main content is created out of the request.
    /// This allows to implement fallback mechanisms in case one handler
    /// is not able to create the requested form.
    /// As a simplified rule: As long as the MainContent is not created, one handler shall try
    /// to create the full form. 
    /// </summary>
    public bool IsMainContentCreated { get; set; }
    
    /// <summary>
    /// Gets or sets a value if the request has been managed.
    /// If the request is not managed until the end of the chain, an error will be sent out
    /// </summary>
    public bool IsManaged { get; set; }
    
    /// <summary>
    /// Gets or sets the information whether the request is finalized
    /// </summary>
    public bool IsFinalized { get; set; }

    public abstract void AddToFormCreationProtocol(string message);

    public static void AddToFormCreationProtocol(IObject form, string message)
    {
        lock (form)
        {   
            var currentMessage =
                form.getOrDefault<string>(_Forms._Form.creationProtocol)
                ?? string.Empty;

            if (currentMessage != string.Empty)
                currentMessage += "\r\n" + message;
            else
                currentMessage = message;

            form.set(_Forms._Form.creationProtocol, currentMessage);
        }
    }
}

public class FormCreationResultOneForm : FormCreationResult
{
    /// <summary>
    /// Gets or sets the result of the activity 
    /// </summary>
    public IElement? Form { get; set; }
    
    /// <summary>
    ///     Adds a certain text to the form creation protocol.
    ///     This protocol is used to allow more easy debugging of the form creation process.
    ///     Otherwise, the form is 'just' there and nobody knows how it was created.
    /// </summary>
    /// <param name="message">Message it self that shall be added</param>
    public override void AddToFormCreationProtocol(string message)
    {
        if (Form == null) return;
        FormCreationResult.AddToFormCreationProtocol(Form, message);
    }
}

public class FormCreationResultMultipleForms : FormCreationResult
{
    /// <summary>
    /// Gets or sets the result of the activity 
    /// </summary>
    public List<IElement> Forms { get; init; } = [];
    
    public override void AddToFormCreationProtocol(string message)
    {
        foreach (var form in Forms)
        {
            FormCreationResult.AddToFormCreationProtocol(form, message);
        }
    }
}