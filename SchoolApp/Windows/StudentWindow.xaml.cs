﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Windows.Student_Windows;

namespace SchoolApp.Windows
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        private readonly SchoolDbContext _context;

        public StudentWindow()
        {
            InitializeComponent();

            _context = App.Context;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await _context.Students.LoadAsync();

            this.StudentsGrid.AddButtonColumn("Edit", EditStudent);
            this.StudentsGrid.AddButtonColumn("Delete", DeleteStudent);

            this.StudentsGrid.BindLocal(_context.Students);
        }

        private void EditStudent(object sender, RoutedEventArgs e)
        {
            Student student = this.StudentsGrid.CurrentCell.Item as Student;

            if (student == null)
            {
                MessageBox.Show("There is no student to edit.");

                return;
            }

            EditStudentPage esp = new EditStudentPage(student, this.StudentsGrid);

            esp.Show();
        }

        private async void DeleteStudent(object sender, RoutedEventArgs e)
        {
            Student student = this.StudentsGrid.CurrentCell.Item as Student;

            if (student == null)
            {
                MessageBox.Show("There is no student to delete.");

                return;
            }

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();

            MessageBox.Show("Student is removed successfully.");

            this.StudentsGrid.BindLocal(_context.Students);
        }

        private void StudentsGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor propertyDescriptor = (PropertyDescriptor) e.PropertyDescriptor;

            e.Column.Header = propertyDescriptor.DisplayName;

            if (propertyDescriptor.DisplayName == "Id")
            {
                e.Cancel = true;
            }
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudentPage asp = new AddStudentPage(this.StudentsGrid);

            asp.Show();
        }
    }
}
