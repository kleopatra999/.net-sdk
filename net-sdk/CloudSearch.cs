﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB
{
    public class SearchFilter
    {
        internal Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
        public SearchFilter()
        {
            
        }

        public SearchFilter NotEqualTo(string columnName, Object data)
        {
            if (columnName.Equals("id") || columnName.Equals("isSearchable") || columnName.Equals("expires"))
            {
                columnName = "_" + columnName;
            }

            var term = new Dictionary<string, Object>();

            if (data.GetType() == typeof(ArrayList))
            {
                term["terms"] = new Dictionary<string, Object>();
                ((Dictionary<string, Object>)term["terms"])[columnName] = data;
            }
            else
            {
                term["term"] = new Dictionary<string, Object>();
                ((Dictionary<string, Object>)term["term"])[columnName] = data;
            }

            this._PushInMustNotFilter(term);

            return this;
        }

        public SearchFilter NotEqualTo(string columnName, Object[] data)
        {
            if (columnName.Equals("id") || columnName.Equals("isSearchable") || columnName.Equals("expires"))
            {
                columnName = "_" + columnName;
            }

            var term = new Dictionary<string, Object>();

            term["terms"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)term["terms"])[columnName] = data;
            
            this._PushInMustNotFilter(term);

            return this;
        }
        
        public SearchFilter EqualTo(string columnName, Object data)
        {
            if (columnName.Equals("id") || columnName.Equals("isSearchable") || columnName.Equals("expires"))
            {
                columnName = "_" + columnName;
            }

            var term = new Dictionary<string, Object>();

            term["term"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)term["term"])[columnName] = data;

            this._PushInMustFilter(term);

            return this;
        }

        public SearchFilter EqualTo(string columnName, Object[] data)
        {
            if (columnName.Equals("id") || columnName.Equals("isSearchable") || columnName.Equals("expires"))
            {
                columnName = "_" + columnName;
            }

            var term = new Dictionary<string, Object>();

            term["terms"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)term["terms"])[columnName] = data;

            this._PushInMustFilter(term);

            return this;
        }

        public SearchFilter Exists(string columnName)
        {
            if (columnName.Equals("id") || columnName.Equals("isSearchable") || columnName.Equals("expires"))
            {
                columnName = "_" + columnName;
            }

            var obj = new Dictionary<string, Object>();
            obj["exists"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["exists"])["field"] = columnName;

            this._PushInMustFilter(obj);

            return this;
        }
        
        public SearchFilter DoesNotExists(string columnName)
        {
            if (columnName.Equals("id") || columnName.Equals("isSearchable") || columnName.Equals("expires"))
            {
                columnName = "_" + columnName;
            }

            var obj = new Dictionary<string, Object>();
            obj["missing"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["missing"])["field"] = columnName;

            this._PushInMustFilter(obj);

            return this;
        }

        public SearchFilter GreaterThan(string columnName, Object data)
        {
            if (columnName.Equals("id") || columnName.Equals("isSearchable") || columnName.Equals("expires"))
            {
                columnName = "_" + columnName;
            }

            var obj = new Dictionary<string, Object>();
            obj["range"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["range"])[columnName] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["range"])[columnName])["gt"] = data;
            this._PushInMustFilter(obj);

            return this;

        }

        public SearchFilter GreaterThanEqualTo(string columnName, Object data)
        {
            if (columnName.Equals("id") || columnName.Equals("isSearchable") || columnName.Equals("expires"))
            {
                columnName = "_" + columnName;
            }

            var obj = new Dictionary<string, Object>();
            obj["range"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["range"])[columnName] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["range"])[columnName])["gte"] = data;
            this._PushInMustFilter(obj);

            return this;

        }

        public SearchFilter LessThan(string columnName, Object data)
        {
            if (columnName.Equals("id") || columnName.Equals("isSearchable") || columnName.Equals("expires"))
            {
                columnName = "_" + columnName;
            }

            var obj = new Dictionary<string, Object>();
            obj["range"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["range"])[columnName] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["range"])[columnName])["lt"] = data;
            this._PushInMustFilter(obj);

            return this;
        }

        public SearchFilter LessThanOrEqualTo(string columnName, Object data)
        {
            if (columnName.Equals("id") || columnName.Equals("isSearchable") || columnName.Equals("expires"))
            {
                columnName = "_" + columnName;
            }

            var obj = new Dictionary<string, Object>();
            obj["range"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["range"])[columnName] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["range"])[columnName])["lte"] = data;
            this._PushInMustFilter(obj);

            return this;
        }

        public void Near(string columnName, CloudGeoPoint geoPoint, double distance)
        {
            var obj = new Dictionary<string, Object>();
            obj["geo_distance"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["geo_distance"])["distance"] = distance;
            ((Dictionary<string, Object>)obj["geo_distance"])["columnName"] = geoPoint.dictionary["coordinates"];

            this._PushInMustFilter(obj);

        }

        public void And(SearchFilter obj)
        {
            if (obj._GetIncludeSize() > 0)
            {
                throw new Exception.CloudBoostException("You cannot have an include over AND. Have an CloudSearch Include over parent SearchFilter instead");
            }

            this._PushInMustFilter(obj);

        }

        public void Or(SearchFilter obj)
        {
            if (obj._GetIncludeSize() > 0)
            {
                throw new Exception.CloudBoostException("You cannot have an include over OR. Have an CloudSearch Include over parent SearchFilter instead");
            }

        }

        public void Not(SearchFilter obj)
        {
            if (obj._GetIncludeSize() > 0)
            {
                throw new Exception.CloudBoostException("You cannot have an include over OR. Have an CloudSearch Include over parent SearchFilter instead");
            }
        }

        public void include(string columnName)
        {
            if (columnName.Equals("id") || columnName.Equals("isSearchable") || columnName.Equals("expires"))
            {
                columnName = "_" + columnName;
            }

            this._PushInInclude(columnName);
        }

        
        /* PRIVATE FUNCTIONS */
        private void _PushInInclude(Object obj)
        {
            ((ArrayList)((Dictionary<string, Object>)dictionary["filter"])["$include"]).Add(obj);
        }

        private int _GetIncludeSize()
        {
            return ((ArrayList)((Dictionary<string, Object>)dictionary["filter"])["$include"]).Count;
        }

        private void _CreateInclude()
        {
            if (((Dictionary<string, Object>)dictionary["filter"])["$include"] == null)
            {
                ((Dictionary<string, Object>)dictionary["filter"])["$include"] = new ArrayList();
            }
        }

        private void _ClearInclude()
        {
            ((ArrayList)((Dictionary<string, Object>)dictionary["filter"])["$include"]).Clear();
        }

        private void _PushInMustFilter(object obj)
        {
            _createBoolFilter();
            ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["filter"])["bool"])["must"]).Add(obj);
        }

        private void _PushInMustNotFilter(object obj)
        {
            _createBoolFilter();
            ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["filter"])["bool"])["must_not"]).Add(obj);
        }

        private void _PushInShouldFilter(object obj)
        {
            _createBoolFilter();
            ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["filter"])["bool"])["should"]).Add(obj);
        }

        private void _createBoolFilter()
        {
            if (((Dictionary<string, Object>)dictionary["filter"])["bool"] == null)
            {
                ((Dictionary<string, Object>)dictionary["filter"])["bool"] = new Dictionary<string, Object>();
            }

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["filtet"])["bool"])["must"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["filter"])["bool"])["must"] = new ArrayList();
            }

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["filter"])["bool"])["should"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["filter"])["bool"])["should"] = new ArrayList();
            }

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["filter"])["bool"])["must_not"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["filter"])["bool"])["must_not"] = new ArrayList();
            }
        }
    }

    public class SearchQuery
    {
        Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
        public SearchQuery()
        {

        }

        public Dictionary<string, Object> _buildSearchPhrase(string columnName, object query, string slop, string boost)
        { 
            var obj = this._buildSearchOn(columnName, query, null, null, null, boost);
            obj["match"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["match"])[columnName] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["match"])[columnName])["type"] = "phrase";
            if (slop != null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["match"])[columnName])["slop"] = slop;
            }
            return obj;
        }

        public Dictionary<string, Object> _buildSearchPhrase(string[] columnName, object query, string slop, string boost)
        {
            var obj = this._buildSearchOn(columnName, query, null, null, null, boost);
            obj["multi_match"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["multi_match"])["type"] = "phrase";
            if (slop != null)
            {
                ((Dictionary<string, Object>)obj["multi_match"])["slop"] = slop;
            }
            return obj;
        }

        public Dictionary<string, Object> _buildBestColumns(string columnName, object query, string fuzziness, string _operator, string match_percent, string boost)
        {
            var obj = this._buildSearchOn(columnName, query, null, null, null, boost);
            obj["match"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["match"])[columnName] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["match"])[columnName])["type"] = "best_fields";
            
            return obj;
        }

        public Dictionary<string, Object> _buildBestColumns(string[] columnName, object query, string fuzziness, string _operator, string match_percent, string boost)
        {
            var obj = this._buildSearchOn(columnName, query, null, null, null, boost);
            obj["multi_match"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["multi_match"])["type"] = "best_fields";
            return obj;
        }

        public Dictionary<string, Object> _buildMostColumns(string columnName, object query, string fuzziness,  string _operator, string match_percent, string boost)
        {
            var obj = this._buildSearchOn(columnName, query, null, null, null, boost);
            obj["match"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["match"])[columnName] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["match"])[columnName])["type"] = "most_fields";
            return obj;
        }

        public Dictionary<string, Object> _buildMostColumns(string[] columnName, object query, string fuzziness, string _operator, string match_percent, string boost)
        {
            var obj = this._buildSearchOn(columnName, query, null, null, null, boost);
            obj["multi_match"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["multi_match"])["type"] = "most_fields";
            return obj;
        }

        public Dictionary<string, Object> _buildSearchOn(string column, object query, string fuzziness, string _operator, string match_percent, string boost)
        {
            var obj = new Dictionary<string, Object>();
            obj["match"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["match"])[column] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["match"])[column])["query"] = query;

            if (_operator != null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["match"])[column])["operator"] = _operator;
            }

            if (match_percent != null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["match"])[column])["minimum_should_match"] = match_percent;
            }

            if (boost != null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["match"])[column])["boost"] = boost;
            }

            if (fuzziness != null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["match"])[column])["fuzziness"] = fuzziness;
            }
            return obj;
        }

        public Dictionary<string, Object> _buildSearchOn(string[] column, object query, string fuzziness, string _operator, string match_percent, string boost)
        {
            var obj = new Dictionary<string, Object>();
            obj["multi_match"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["multi_match"])["query"] = query;

            if (_operator != null)
            {
                ((Dictionary<string, Object>)obj["multi_match"])["operator"] = _operator;
            }

            if (match_percent != null)
            {
                ((Dictionary<string, Object>)obj["multi_match"])["minimum_should_match"] = match_percent;
            }

            if (boost != null)
            {
                ((Dictionary<string, Object>)obj["multi_match"])["boost"] = boost;
            }

            if (fuzziness != null)
            {
                ((Dictionary<string, Object>)obj["multi_match"])["fuzziness"] = fuzziness;
            }
            return obj;
        }

        public CB.SearchQuery SearchOn(string columns, object query, string fuzziness, string all_words, string match_percent, string priority)
        {
            if (all_words != null)
            {
                all_words = "and";
            }
            var obj = this._buildSearchOn(columns, query, fuzziness, all_words, match_percent, priority);
            this._PushInShouldQuery(obj);
            return this;
        }

        public CB.SearchQuery Phrase(string columns, object query, string fuzziness, string priority)
        {
            var obj = this._buildSearchPhrase(columns, query, fuzziness, priority);
            this._PushInShouldQuery(obj);
            return this;
        }

        public CB.SearchQuery BestColumns(string[] columns, object query, string fuzziness, string all_words, string match_percent, string priority)
        {
            if (all_words != null)
            {
                all_words = "and";
            }
            var obj = this._buildBestColumns(columns, query, fuzziness, all_words, match_percent, priority);
            this._PushInShouldQuery(obj);
            return this;
        }

        public CB.SearchQuery MostColumns(string[] columns, object query, string fuzziness, string all_words, string match_percent, string priority)
        {
            if (all_words != null)
            {
                all_words = "and";
            }
            var obj = this._buildMostColumns(columns, query, fuzziness, all_words, match_percent, priority);
            this._PushInShouldQuery(obj);
            return this;
        }

        public void StartsWith(string column, object value, int? priority)
        {
            var obj = new Dictionary<string, Object>();
            obj["prefix"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["prefix"])[column] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["prefix"])[column])["value"] = value;
            if (priority != null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["prefix"])[column])["boost"] = priority;

            }
            this._PushInMustQuery(obj);
        }

        public void WildCard(string column, object value, int? priority)
        {
            var obj = new Dictionary<string, Object>();
            obj["wildcard"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["wildcard"])[column] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["wildcard"])[column])["value"] = value;
            if (priority != null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["wildcard"])[column])["boost"] = priority;

            }
            this._PushInShouldQuery(obj);
        }

        public void RegExp(string column, object value, int? priority)
        {
            var obj = new Dictionary<string, Object>();
            obj["regexp"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["regexp"])[column] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["regexp"])[column])["value"] = value;
            if (priority != null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["regexp"])[column])["boost"] = priority;

            }

            this._PushInMustQuery(obj);
        }

        public void And(CB.SearchQuery obj)
        {
            this._PushInMustQuery(obj);
        }

        public void Or(CB.SearchQuery obj)
        {
            this._PushInShouldQuery(obj);
        }

        public void Not(CB.SearchQuery obj)
        {
            this._PushInMustNotQuery(obj);
        }

        private void _PushInMustQuery(object obj)
        {
            _createBoolQuery();
            ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["must"]).Add(obj);
        }

        private void _PushInMustNotQuery(object obj)
        {
            _createBoolQuery();
            ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["must_not"]).Add(obj);
        }

        private void _PushInShouldQuery(object obj)
        {
            _createBoolQuery();
            ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["should"]).Add(obj);
        }

        private void _createBoolQuery()
        {
            if ( ((Dictionary<string, Object>)dictionary["query"])["bool"] == null )
            {
                ((Dictionary<string, Object>)dictionary["query"])["bool"] = new Dictionary<string, Object>();
            }

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["must"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["must"] = new ArrayList();
            }

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["should"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["should"] = new ArrayList();
            }

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["must_not"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["must_not"] = new ArrayList();
            }
        }
    }

    public class CloudSearch
    {
        internal Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
        public SearchQuery SearchQuery;
        public SearchFilter SearchFilter;
        public CloudSearch(string tableName)
        {
            dictionary["collectionNames"] = tableName;
            dictionary["query"] = new Dictionary<string, Object>();
            dictionary["from"] = 0;
            dictionary["size"] = 10;
            dictionary["sort"] = new ArrayList();
        }

        public CloudSearch(ArrayList tableNames)
        {
            dictionary["collectionNames"] = tableNames;
            dictionary["query"] = new Dictionary<string, Object>();
            dictionary["from"] = 0;
            dictionary["size"] = 10;
            dictionary["sort"] = new ArrayList();
        }

        public int Skip
        {
            get
            {
                return (int)dictionary["from"];
            }
            set
            {
                dictionary["from"] = value;
            }
        }

        public int Limit
        {
            get
            {
                return (int)dictionary["size"];
            }
            set
            {
                dictionary["size"] = value;
            }
        }

        public CloudSearch OrderByAsc(string column)
        {
            Dictionary<string, Object> temp = new Dictionary<string, object>();
            temp[column] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)temp[column])["order"] = "asc";
            ((ArrayList)dictionary["sort"]).Add(temp);

            return this;
        }

        public CloudSearch OrderByDesc(string column)
        {
            Dictionary<string, Object> temp = new Dictionary<string, object>();
            temp[column] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)temp[column])["order"] = "desc";
            ((ArrayList)dictionary["sort"]).Add(temp);

            return this;
        }
     
        public CloudSearch SearchOn(ArrayList columns, object query)
        {
            if (this._IsFilteredQuery)
            {
                //if columns is an array.
                (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["query"])["multi_match"]) = new Dictionary<string, object>();
                (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["query"])["multi_match"])["query"]) = query;
                (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["query"])["multi_match"])["fields"]) = columns;
            }
            else
            {
                //if columns is an array.
                ((((Dictionary<string, Object>)dictionary["query"]))["multi_match"]) = new Dictionary<string, object>();
                (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["multi_match"])["query"]) = query;
                (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["multi_match"])["fields"]) = columns;
            }

            return this;
        }


        public async Task<ArrayList> Search()
        {
            var result = await Util.CloudRequest.Send("/search", dictionary);
            return (ArrayList)result;
        }
        public static CloudSearch Or(CloudSearch searchObj1, CloudSearch searchObj2)
        {

            var collectionNames = new ArrayList();

            if (searchObj1.dictionary["collectionNames"].GetType() == typeof(ArrayList))
            {
                collectionNames.AddRange((ArrayList)searchObj1.dictionary["collectionNames"]);
            }
            else
            {
                collectionNames.Add((ArrayList)searchObj1.dictionary["collectionNames"]);
            }

            if (searchObj2.dictionary["collectionNames"].GetType() == typeof(ArrayList))
            {
                //check for duplicates.
                for (var i = 0; i < ((ArrayList)searchObj2.dictionary["collectionNames"]).Count; i++)
                {
                    if (collectionNames.IndexOf(((ArrayList)searchObj2.dictionary["collectionNames"])[i]) < 0)
                        collectionNames.Add(((ArrayList)searchObj2.dictionary["collectionNames"])[i]);
                }
            }
            else
            {
                if (collectionNames.IndexOf(((string)searchObj2.dictionary["collectionNames"])) < 0)
                    collectionNames.Add(((ArrayList)searchObj2.dictionary["collectionNames"]));
            }

            var obj3 = new CB.CloudSearch(collectionNames);
            //merge both of the objects.

            Dictionary<string, Object> q1 = null;
            Dictionary<string, Object> q2 = null;
            Dictionary<string, Object> f1 = null;
            Dictionary<string, Object> f2 = null;

            if (((((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])) != null && (((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])["query"]) != null)
            {
                q1 = (Dictionary<string, Object>)(((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])["query"]);
            }
            else if (searchObj1.dictionary["query"] != null && ((((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])) == null)
            {
                q1 = (Dictionary<string, Object>)searchObj1.dictionary["query"];
            }

            if (((((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])) != null && (((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])["query"]) != null)
            {
                q2 = (Dictionary<string, Object>)(((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])["query"]);
            }
            else if (searchObj2.dictionary["query"] != null && ((((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])) == null)
            {
                q2 = (Dictionary<string, Object>)searchObj2.dictionary["query"];
            }


            if (((((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])) != null && (((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])["filter"]) != null)
                f1 = (Dictionary<string, Object>)(((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])["filter"]);

            if (((((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])) != null && (((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])["filter"]) != null)
                f2 = (Dictionary<string, Object>)(((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])["filter"]);

            if (f1.Count > 0 || f2.Count > 0)
            { //if any of the filters exist, then...
                obj3._MakeFilteredQuery();
                if (f1.Count > 0 && f2.Count == 0)
                    (((Dictionary<string, Object>)((Dictionary<string, Object>)obj3.dictionary["query"])["filteredQuery"])["filter"]) = f1;
                else if (f2.Count > 0 && f1.Count == 0)
                    (((Dictionary<string, Object>)((Dictionary<string, Object>)obj3.dictionary["query"])["filteredQuery"])["filter"]) = f2;
                else
                {
                    //if both exists.
                    obj3._PushInShouldFilter(f1);
                    obj3._PushInShouldFilter(f2);
                }
            }
            else
            {
                //only query exists.
                obj3._CreateBoolQuery();
                obj3._PushInShouldQuery(q1);
                obj3._PushInShouldQuery(q2);
            }

            return obj3;

        }
    }
}
