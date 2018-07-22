using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonkeyCache;

namespace TodoApp.Repositories
{
	public abstract class BaseRepository
	{
		protected IBarrel Barrel { get; }

		readonly DateTime ExpirationDate;

		public BaseRepository(IBarrel barrel)
		{
			Barrel = barrel;
			// arbiratry expiration date for all the data
			ExpirationDate = new DateTime(2019, 12, 31);
		}

		protected async Task<IEnumerable<TModel>> Get<TModel>(string key) where TModel : Models.BaseModel
		{
			// wrap in task so the file system is accessed on a background thread
			return await Task.Run(() => Barrel.Get<IEnumerable<TModel>>(key));
		}

		protected async Task<TModel> Get<TModel>(string key, Guid id) where TModel : Models.BaseModel
		{
			// wrap in task so the file system is accessed on a background thread
			return await Task.Run(() => Barrel.Get<IEnumerable<TModel>>(key)?.FirstOrDefault(x => x.Id == id));
		}

		protected async Task<TModel> Create<TModel>(string listKey, TModel model) where TModel : Models.BaseModel
		{
			// generate a new id, using so we don't have to check for existing numerics
			model.Id = Guid.NewGuid();

			// get data currenlty stored
			var listOfModels = (await Get<TModel>(listKey))?.ToList() ?? new List<TModel>();

			listOfModels.Add(model);

			await Save(listKey, listOfModels);

			return model;
		}

		protected async Task Update<TModel>(string listKey, Guid id, TModel model) where TModel : Models.BaseModel
		{
			// get data currenlty stored
			var listOfModels = (await Get<TModel>(listKey))?.ToList() ?? new List<TModel>();

			var matching = listOfModels.FirstOrDefault(x => x.Id == id);
			if (matching != null)
			{
				var index = listOfModels.IndexOf(matching);
				listOfModels.RemoveAt(index);
				listOfModels.Insert(index, model);
			}
			else
			{
				// play it safe, if there isn't an existing model, do a create with that id
				model.Id = id;
				listOfModels.Add(model);
			}

			await Save(listKey, listOfModels);
		}

		protected async Task Delete<TModel>(string listKey, Guid id) where TModel : Models.BaseModel
		{
			// get data currenlty stored
			var listOfModels = (await Get<TModel>(listKey))?.ToList() ?? new List<TModel>();

			var matching = listOfModels.FirstOrDefault(x => x.Id == id);
			if (matching != null)
			{
				var index = listOfModels.IndexOf(matching);
				listOfModels.RemoveAt(index);

				await Save(listKey, listOfModels);
			}
		}

		protected async Task Save<TValue>(string key, TValue value)
		{
			// access file system on background thread
			await Task.Run(() => Barrel.Add(key, value, ExpirationDate - DateTime.Now));
		}
	}
}
