import {UserEvent} from "../../burnsystems/Events.js";
import {ItemWithNameAndId} from "../ApiModels.js";

/**
 * Common contract shared by every "select item" sub-control (browse, freetext,
 * last history and by the container itself.
 *
 * Integrators should program against this interface so that the concrete
 * implementation can be swapped without touching call sites. The container
 * forwards pre-selection calls (`setWorkspaceById`, `setExtentByUri`,
 * `setItemByUri`) to the currently active sub-control and re-raises its
 * selection events.
 */
export interface ISelectItemControl {
    
    /**
     * Fired when the user has highlighted/clicked an item in the list without
     * yet confirming the selection. Use this to react to navigation, e.g. to
     * update a preview pane. Does not imply the user is done choosing.
     */
    itemClicked: UserEvent<ItemWithNameAndId>;

    /**
     * Pre-selects the given workspace. May be called before the workspaces
     * have been loaded; in that case the pre-selection is applied as soon as
     * the GUI is initialized.
     *
     * @param workspaceId ID of the workspace to pre-select.
     * @returns A promise that resolves once the GUI reflects the selection.
     */
    setWorkspaceById(workspaceId: string) : Promise<void>;

    /**
     * Pre-selects the given extent (and its hosting workspace). May be called
     * before workspaces/extents have been loaded.
     *
     * @param workspaceId ID of the hosting workspace.
     * @param extentUri URI of the extent to pre-select.
     * @returns A promise that resolves once the GUI reflects the selection.
     */
    setExtentByUri(workspaceId: string, extentUri: string): Promise<void>;

    /**
     * Pre-selects a specific item by its URI. The hosting workspace and extent
     * are resolved from the item and pre-selected too. May be called before
     * the GUI has been initialized; the pre-selection is then applied on
     * first load.
     *
     * @param workspaceId ID of the workspace containing the item.
     * @param itemUri URI of the item to pre-select.
     * @returns A promise that resolves once the GUI reflects the selection.
     * @throws When no item can be resolved for the given workspace/URI pair.
     */
    setItemByUri?(workspaceId: string, itemUri: string) : Promise<void>;

    /**
     * Returns the currently selected item, or `undefined` if nothing is
     * selected. The returned object is a snapshot — mutating it does not
     * change the control's internal state.
     */
    getSelectedItem(): ItemWithNameAndId;

}