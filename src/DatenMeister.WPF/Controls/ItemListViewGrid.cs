using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using BurnSystems.Collections;
using DatenMeister.WPF.Controls.GridControl;

namespace DatenMeister.WPF.Controls;

/// <summary>
/// Create the grid view
/// </summary>
public class ItemListViewGrid : DockPanel
{
    private readonly Canvas _canvas = new();
    private readonly ScrollBar _scrollVertical = new ScrollBar();
    private readonly ScrollBar _scrollHorizontal = new ScrollBar();
    private readonly DockPanel _contentPanel = new();
        
    private readonly List<RowInstantiation> _rowInstantiations = new();

    private readonly List<ColumnInstantiation> _columnInstantiations = new();
    private int _rowOffset;
    private int _columnOffset;
    private double _columnPixelOffset;

    /// <summary>
    /// Gets or sets the index of the selected row
    /// </summary>
    public int SelectedDataIndex { get; set; } = -1;

    /// <summary>
    /// Gets or sets the selected row
    /// </summary>
    public RowInstantiation? SelectedRow =>
        _rowInstantiations.FirstOrDefault(x => x.IndexInData == SelectedDataIndex);

    /// <summary>
    /// Defines the data that has been selected.
    /// This value is used to find the selected item in case of a new setting of data
    /// </summary>
    private object? _selectedRowData;

    /// <summary>
    /// Defines some default grid settings
    /// </summary>
    public static class GridSettings
    {
        public const double BorderWidth = 1.0;
        public const double GridPenWidth = 1.0;
        public static readonly Brush GridBrush = Brushes.DimGray;
        public static readonly Brush SelectedRowBrush = Brushes.LightSteelBlue;
        public static readonly Thickness GridMargin = new(5, 2, 5, 1);
        public const double HeaderRowHeight = 20.0;
    }

    /// <summary>
    /// Gets the list of columndefinitions
    /// </summary>
    public DirtyList<GridColumnDefinition> ColumnDefinitions { get; } = new();

    static ItemListViewGrid()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemListViewGrid),
            new FrameworkPropertyMetadata(typeof(ItemListViewGrid)));
    }

    protected override void OnInitialized(EventArgs e)
    {
        Background = Brushes.Transparent;

        //  Create the scroll bars
        _scrollVertical.Orientation = Orientation.Vertical;
        _scrollVertical.Minimum = 0;
        _scrollVertical.SmallChange = 1;
        _scrollVertical.LargeChange = 3;
        _scrollVertical.ViewportSize = 1.0;
        _scrollVertical.Margin = new Thickness(
            0,
            GridSettings.HeaderRowHeight
            + GridSettings.GridMargin.Bottom
            + GridSettings.GridMargin.Top
            + GridSettings.GridPenWidth
            + GridSettings.BorderWidth,
            0, 0);
        _scrollVertical.Margin = new Thickness(0,
            GridSettings.GridMargin.Top + GridSettings.GridMargin.Bottom + GridSettings.HeaderRowHeight,
            0, 0);
        _scrollVertical.ValueChanged += (x, y) =>
        {
            _rowOffset = Convert.ToInt32(_scrollVertical.Value);
            InvalidateVisual();
            InvalidateMeasure();
        };
            
        _scrollHorizontal.Orientation = Orientation.Horizontal;
        _scrollHorizontal.Minimum = 0;
        _scrollHorizontal.SmallChange = 1;
        _scrollHorizontal.LargeChange = 3;
        _scrollHorizontal.ViewportSize = 1.0;

        _scrollHorizontal.ValueChanged += (x, y) =>
        {
            _columnOffset = Convert.ToInt32(_scrollHorizontal.Value);
            _columnPixelOffset = GetColumnInstantiation(_columnOffset).OffsetWidth;
            InvalidateVisual();
            InvalidateMeasure();
        };

        _canvas.ClipToBounds = true;

        // Creates the content panel
        _contentPanel.Children.Add(_scrollHorizontal);
        _contentPanel.Children.Add(_scrollVertical);
        _contentPanel.Children.Add(_canvas);
        SetDock(_scrollVertical, Dock.Right);
        SetDock(_scrollHorizontal, Dock.Bottom);

        // Creates the border around
        var border = new Border
        {
            BorderThickness = new Thickness(GridSettings.BorderWidth),
            BorderBrush = GridSettings.GridBrush,
            Child = _contentPanel,
            UseLayoutRounding = true
        };

        Children.Add(border);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        _scrollVertical.Value -= e.Delta / 120.0;
        base.OnMouseWheel(e);
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        var position = e.GetPosition(_canvas);
        if (position.Y < 0 || position.Y > _canvas.ActualHeight)
        {
            return;
        }

        RowInstantiation? lastRow = null;
        foreach (var rowInstantiation in _rowInstantiations)
        {
            if (rowInstantiation.OffsetHeight < position.Y
                && rowInstantiation.OffsetHeight + rowInstantiation.Height > position.Y)
            {
                lastRow = rowInstantiation;
            }
        }

        if (lastRow != null)
        {
            SelectedDataIndex = lastRow.IndexInData;
            _selectedRowData = GetDataOfRow(lastRow.IndexInData);
            InvalidateVisual();
            InvalidateMeasure();
        }
        else
        {
            SelectedDataIndex = -1;
            _selectedRowData = null;
            InvalidateVisual();
            InvalidateMeasure();
        }
            
        base.OnMouseDown(e);
    }

    /// <summary>
    /// Goes through the data of the element and tries to set 
    /// </summary>
    protected void FindSelectedElement()
    {
        if (_selectedRowData == null)
        {
            return;
        }
            
        for (var n = 0; n < DataRowCount; n++)
        {
            var current = GetDataOfRow(n);
            if (current == null) continue;

            if (current.Equals(_selectedRowData))
            {
                SelectedDataIndex = n;
            }
        }
    }

    /// <summary>
    /// Gets the cell instantiation of a certain cell and row
    /// </summary>
    /// <param name="row">Row to be evaluated</param>
    /// <param name="column">Column to be evaluation</param>
    /// <returns></returns>
    public CellInstantiation GetCellInstantiation(int row, int column)
    {
        while (_rowInstantiations.Count <= row)
        {
            _rowInstantiations.Add(new RowInstantiation());
        }

        var rowInstantiation = _rowInstantiations[row];

        while (rowInstantiation.Cells.Count <= column)
        {
            rowInstantiation.Cells.Add(new CellInstantiation());
        }

        return rowInstantiation.Cells[column];
    }

    /// <summary>
    /// Gets the column instantiations
    /// </summary>
    /// <param name="column">Number of column</param>
    /// <returns>The retrieved column</returns>
    public ColumnInstantiation GetColumnInstantiation(int column)
    {
        while (_columnInstantiations.Count <= column)
        {
            _columnInstantiations.Add(new ColumnInstantiation());
        }

        return _columnInstantiations[column];
    }

    /// <summary>
    /// Gets the row instantiations
    /// </summary>
    /// <param name="row">Number of row</param>
    /// <returns>The retrieved column</returns>
    public RowInstantiation GetRowInstantiation(int row)
    {
        while (_rowInstantiations.Count <= row)
        {
            _rowInstantiations.Add(new RowInstantiation());
        }

        return _rowInstantiations[row];
    }

    public RowInstantiation GetHeaderRowInstantiation()
    {
        var result = new RowInstantiation
        {
            Height = GridSettings.HeaderRowHeight,
            BackgroundColor = Brushes.LightGray
        };

        foreach (var column in ColumnDefinitions)
        {
            var cell = new CellInstantiation
            {
                CellElement = new TextBlock
                {
                    Text = column.Title,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };

            result.Cells.Add(cell);
        }

        return result;
    }

    protected override Size MeasureOverride(Size constraint)
    {
        _rowInstantiations.Clear();
        _canvas.Children.Clear();

        // Create the Column Instantiations
        var offsetColumn = 0.0;

        if (ColumnDefinitions.PopDirty())
        {
            _columnInstantiations.Clear();
            foreach (var definition in ColumnDefinitions)
            {
                _columnInstantiations.Add(
                    new ColumnInstantiation
                    {
                        OffsetWidth = offsetColumn,
                        ColumnDefinition = definition
                    });
            }
        }

        // Go through the rows and estimate the sizes
        var positionRow = 0.0;
        for (var n = 0; n < DataRowCount + 1; n++)
        {
            var rowInstantiation = GetVisibleRow(n);
            if (rowInstantiation != null)
            {
                if (rowInstantiation.Height == 0) rowInstantiation.Height = GridSettings.HeaderRowHeight;

                rowInstantiation.OffsetHeight = positionRow;
                _rowInstantiations.Add(rowInstantiation);

                var c = 0;
                foreach (var cell in rowInstantiation.Cells)
                {
                    var columnInstantiation = GetColumnInstantiation(c);
                    if (cell.CellElement != null)
                    {
                        cell.CellElement.OverridesDefaultStyle = true;
                        cell.CellElement.MaxHeight = rowInstantiation.Height;
                        cell.CellElement.MaxWidth = constraint.Width / 2.0; // Limit horizontal size by maximum of half-size
                        cell.CellElement.Measure(new Size(double.MaxValue, rowInstantiation.Height));

                        columnInstantiation.DesiredWidth =
                            Math.Min(constraint.Width / 2.0,
                                Math.Max(
                                    columnInstantiation.DesiredWidth,
                                    cell.CellElement.DesiredSize.Width));
                        columnInstantiation.DesiredHeight =
                            Math.Max(
                                columnInstantiation.DesiredHeight,
                                cell.CellElement.DesiredSize.Height);
                        _canvas.Children.Add(cell.CellElement);
                    }

                    c++;
                }

                positionRow += rowInstantiation.Height + GridSettings.GridPenWidth
                                                       + GridSettings.GridMargin.Top
                                                       + GridSettings.GridMargin.Bottom;
            }

            if (positionRow > constraint.Height)
            {
                break;
            }
        }
            
        // Third step. Take the desired sizes and assign them to the columns
        var y = 0;
        foreach (var definition in ColumnDefinitions)
        {
            var current = GetColumnInstantiation(y);
            current.OffsetWidth = offsetColumn;
            current.Width = Math.Max(
                Math.Max(
                    current.Width,
                    Math.Ceiling(definition.DesiredWidth)),
                Math.Ceiling(current.DesiredWidth));

            offsetColumn += current.Width;
            offsetColumn += GridSettings.GridPenWidth
                            + GridSettings.GridMargin.Left
                            + GridSettings.GridMargin.Right;
            y++;
        }

        return base.MeasureOverride(constraint);
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        foreach (var rowInstantiation in _rowInstantiations)
        {
            var n = 0;
            foreach (var column in rowInstantiation.Cells)
            {
                var columnInstantiation = GetColumnInstantiation(n);
                if (column.CellElement != null)
                {
                    column.CellElement.Arrange(
                        new Rect(0, 0, columnInstantiation.Width, rowInstantiation.Height));

                    var heightAlignment = Math.Max(
                        0,
                        (rowInstantiation.Height - column.CellElement.ActualHeight) / 2.0);

                    Canvas.SetTop(column.CellElement,
                        rowInstantiation.OffsetHeight 
                        + GridSettings.GridMargin.Top 
                        + heightAlignment);
                    Canvas.SetLeft(column.CellElement,
                        columnInstantiation.OffsetWidth
                        + GridSettings.GridMargin.Left
                        - _columnPixelOffset);
                }

                n++;
            }
        }

        _scrollHorizontal.Maximum = _columnInstantiations.Count - 1;
        _scrollVertical.Maximum = DataRowCount - 1;
            
        return base.ArrangeOverride(arrangeSize);
    }

    /// <summary>
    /// Returns the number of columns
    /// </summary>
    public int ColumnCount => ColumnDefinitions.Count;

    /// <summary>
    /// Returns the number of columns
    /// </summary>
    public virtual int DataRowCount => 10;

    /// <summary>
    /// Gets the row of the content. This method shall be overridden
    /// </summary>
    /// <param name="dataRow">Number of the row to be queried</param>
    /// <returns>The row instantiation including the content to be shown</returns>
    public virtual RowInstantiation? GetRowOfContent(int dataRow)
    {
        // 
        // THIS IS A DEMO IMPLEMENTATION 
        //
            
        // The instantiated row
        var rowInstantiation = new RowInstantiation();
        var c = 0;
        foreach (var _ in _columnInstantiations)
        {
            var cell = new CellInstantiation
            {
                CellElement = new TextBlock
                {
                    Text = $"R #{dataRow}, C #{c}",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                }
            };

            rowInstantiation.Cells.Add(cell);
            c++;
        }

        return rowInstantiation;
    }

    /// <summary>
    /// Gets an enumeration of the selected items
    /// </summary>
    public IEnumerable<object> SelectedItems
    {
        get
        {
            if (SelectedRow != null)
            {
                var data = GetDataOfRow(SelectedRow.IndexInData);
                if (data != null)
                {
                    return new[] {data};
                }
            }

            return Array.Empty<object>();
        }
    }

    /// <summary>
    /// Gets the object behind the given data row number
    /// </summary>
    /// <param name="dataRow">Index within the data for the given row.
    /// This number is the same number as specified by GetRowOfContent and does not include
    /// any scrolling or zooming</param>
    /// <returns>The object behind the data row</returns>
    public virtual object? GetDataOfRow(int dataRow)
    {
        return null;
    }

    /// <summary>
    /// Returns the instantiation of the visible row of the content.
    /// The first (0) is usually the header row, then followed by the
    /// content rows
    /// </summary>
    /// <param name="row">Number of row to be shown</param>
    /// <returns>The instantiated row or null if not available</returns>
    public RowInstantiation? GetVisibleRow(int row)
    {
        static bool IsHeaderRow(int theRow) => theRow == 0;
        int MapRows(int theRow) => theRow == 0 ? theRow - 1 : theRow + _rowOffset - 1;

        if (IsHeaderRow(row))
        {
            var foundRow = GetHeaderRowInstantiation();
            foundRow.IndexInData = -1;
            return foundRow;
        }
        else
        {
            var index = MapRows(row);
            var foundRow = GetRowOfContent(index);
            if (foundRow != null)
            {
                foundRow.IndexInData = index;
            }

            return foundRow;
        }
    }

    /// <summary>
    /// Gets the number of real visible rows
    /// </summary>
    /// <returns></returns>
    public int GetVisibleRowCount()
    {
        return DataRowCount + 1;
    }

    protected override void OnRender(DrawingContext dc)
    {
        double ResidualWidth(double currentWidth) => _canvas.ActualWidth - currentWidth;
        double ResidualHeight(double currentHeight) => _canvas.ActualHeight - currentHeight;
        var w = GridSettings.BorderWidth;

        var blackPen = new Pen(GridSettings.GridBrush, GridSettings.GridPenWidth);

        // Goes through the rows

        var lastRowHeight = 0.0;
            
        // First of all, paints the rows
        foreach (var rowInstantiation in _rowInstantiations)
        {
            // Skip the row, if not existing
            if (rowInstantiation == null) continue;

            if (ResidualHeight(rowInstantiation.OffsetHeight) < 0)
            {
                break;
            }

            var bottomHeight =
                rowInstantiation.OffsetHeight
                + rowInstantiation.Height
                + GridSettings.GridMargin.Top
                + GridSettings.GridMargin.Bottom;

            var currentBackgroundColor = rowInstantiation.BackgroundColor;
            if (rowInstantiation.IndexInData == SelectedRow?.IndexInData && rowInstantiation.IndexInData != -1)
            {
                currentBackgroundColor = GridSettings.SelectedRowBrush;
            }

            // Sets the backgroundcolor
            if (currentBackgroundColor != null)
            {
                dc.DrawRectangle(
                    currentBackgroundColor,
                    null,
                    new Rect(
                        w,
                        rowInstantiation.OffsetHeight + w,
                        _canvas.ActualWidth,
                        rowInstantiation.Height
                        + GridSettings.GridMargin.Top
                        + GridSettings.GridMargin.Bottom));
            }

            lastRowHeight = Math.Max(lastRowHeight, bottomHeight + w + GridSettings.GridPenWidth / 2.0);
            dc.DrawLine(
                blackPen,
                new Point(0 + w, bottomHeight + w + GridSettings.GridPenWidth / 2.0),
                new Point(_canvas.ActualWidth + w, bottomHeight + w + GridSettings.GridPenWidth / 2.0));

            base.OnRender(dc);
        }

        // Goes through the columns
        for (var currentColumn = _columnOffset; currentColumn < ColumnCount; currentColumn++)
        {
            var columnInstantiation = GetColumnInstantiation(currentColumn);
            var rightWidth = 
                columnInstantiation.OffsetWidth
                + columnInstantiation.Width
                + GridSettings.GridMargin.Left 
                + GridSettings.GridMargin.Right 
                + GridSettings.GridPenWidth 
                - _columnPixelOffset; 
                
            if (ResidualWidth(rightWidth) < 0)
            {
                break;
            }

            dc.DrawLine(
                blackPen,
                new Point(rightWidth + w - GridSettings.GridPenWidth / 2.0, w),
                new Point(rightWidth + w - GridSettings.GridPenWidth / 2.0, lastRowHeight + w));
        }
    }
}