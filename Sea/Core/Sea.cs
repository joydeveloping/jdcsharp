﻿// Author: Alexey Rybakov

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lib.DataStruct;
using Sea.Core.Authors;
using Sea.Core.Publishers;
using Sea.Core.Categories;
using Sea.Core.Books;
using Sea.Tools;

namespace Sea.Core
{
    /// <summary>
    /// Sea class.
    /// </summary>
    public class Sea
    {
        /// <summary>
        /// Authors list.
        /// </summary>
        public AuthorsList Authors;

        /// <summary>
        /// Publishers list.
        /// </summary>
        public PublishersList Publishers;

        /// <summary>
        /// Category root.
        /// </summary>
        public MPTTTree CategoryRoot;

        /// <summary>
        /// Books list.
        /// </summary>
        public BooksList Books;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Sea()
        {
        }

        /// <summary>
        /// Serialization.
        /// </summary>
        public void Serialize()
        {
            SerializeAuthors();
            SerializePublishers();
            SerializeCategories();
            SerializeBooks();
        }

        /// <summary>
        /// Serialize authors.
        /// </summary>
        public void SerializeAuthors()
        {
            Authors.XmlSerialize(Parameters.AuthorsXMLFullFilename);
        }

        /// <summary>
        /// Serialize publishers.
        /// </summary>
        public void SerializePublishers()
        {
            Publishers.XmlSerialize(Parameters.PublishersXMLFullFilename);
        }

        /// <summary>
        /// Serialize categories.
        /// </summary>
        public void SerializeCategories()
        {
            CategoryRoot.XmlSerialize(Parameters.CategoriesTreeXMLFullFilename);
        }

        /// <summary>
        /// Serialize books.
        /// </summary>
        public void SerializeBooks()
        {
            Books.XmlSerialize(Parameters.BooksXMLFullFilename);
        }

        /// <summary>
        /// Deserialize sea.
        /// </summary>
        public void Deserialize()
        {
            DeserializeAuthors();
            DeserializePublishers();
            DeserializeCategories();
            DeserializeBooks();
        }

        /// <summary>
        /// Deserialize authors.
        /// </summary>
        public void DeserializeAuthors()
        {
            Authors = AuthorsList.XmlDeserialize(Parameters.AuthorsXMLFullFilename);

            if (Authors == null)
            {
                Authors = new AuthorsList();
            }
        }

        /// <summary>
        /// Deserialize publishers.
        /// </summary>
        public void DeserializePublishers()
        {
            Publishers = PublishersList.XmlDeserialize(Parameters.PublishersXMLFullFilename);

            if (Publishers == null)
            {
                Publishers = new PublishersList();
            }
        }

        /// <summary>
        /// Deserialize categories.
        /// </summary>
        public void DeserializeCategories()
        {
            CategoryRoot = MPTTTree.XmlDeserialize(Parameters.CategoriesTreeXMLFullFilename);

            if (CategoryRoot == null)
            {
                // Empty tree.
                CategoryRoot = new MPTTTree(" ");
            }
        }

        /// <summary>
        /// Deserialize books.
        /// </summary>
        public void DeserializeBooks()
        {
            Books = BooksList.XmlDeserialize(Parameters.BooksXMLFullFilename, Authors, Publishers, CategoryRoot);

            if (Books == null)
            {
                Books = new BooksList();
            }
        }

        /// <summary>
        /// Authors fixing.
        /// </summary>
        /// <param name="is_approved">approve flag</param>
        public void FixAuthors(bool is_approved)
        {
            if (is_approved)
            {
                SerializeAuthors();
            }
            else
            {
                DeserializeAuthors();
            }
        }

        /// <summary>
        /// Publishers fixing.
        /// </summary>
        /// <param name="is_approved">approve flag</param>
        public void FixPublishers(bool is_approved)
        {
            if (is_approved)
            {
                SerializePublishers();
            }
            else
            {
                DeserializePublishers();
            }
        }

        /// <summary>
        /// Categories fixing.
        /// </summary>
        /// <param name="is_approved">approve flag</param>
        public void FixCategories(bool is_approved)
        {
            if (is_approved)
            {
                SerializeCategories();
            }
            else
            {
                DeserializeCategories();
            }
        }

        /// <summary>
        /// Books fixing.
        /// </summary>
        /// <param name="is_approved">approve flag</param>
        public void FixBooks(bool is_approved)
        {
            if (is_approved)
            {
                SerializeBooks();
            }
            else
            {
                DeserializeBooks();
            }
        }
    }
}