﻿using System.Collections.Generic;

namespace FFramework.MVVM.UnityEditor
{

    /// <summary>
    /// 用于过滤表格注释
    /// </summary>
    [ExcelFilter(typeof(NoteFilter),0)]
    public class NoteFilter : SyntaxFilter<string>
    {
        readonly HashSet<int> filterRow = new HashSet<int>();
        readonly HashSet<int> filterColumn = new HashSet<int>();

        const string note = "#";

        private static bool IsNote(string text)
        {
            return text.StartsWith(note);
        }

        public override ConfigTable<string> GetNextTable(ConfigTable<string> table)
        {
            ConfigTable<string> temp = new ConfigTable<string>(table.RowCount, table.ColumnCount);
            int row = 0, column = 0;

            //行列注释筛选

            for (int i = 0; i < table.RowCount; i++)
                if (IsNote(table[i, 0]) && !filterRow.Contains(i))
                    filterRow.Add(i);
            for (int i = 0; i < table.ColumnCount; i++)
                if (IsNote(table[0, i]) && !filterColumn.Contains(i))
                    filterColumn.Add(i);

            //过滤拷贝
            for (int i = 0; i < table.RowCount; ++i)
            {
                if (filterRow.Contains(i)) continue;
                for (int j = 0; j < table.ColumnCount; ++j)
                {
                    if (filterColumn.Contains(j)) continue;
                    temp[row, column] = table[i, j];
                    column++;
                }
                column = 0;
                row++;
            }
            temp.SetSizeAndCopy(table.RowCount - filterRow.Count, table.ColumnCount - filterColumn.Count, true);
            filterRow.Clear();
            filterColumn.Clear();

            return temp;
        }
    }
}